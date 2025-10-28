using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TestDISC.Actions;
using TestDISC.Actions.Interfaces;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.Auth;
using TestDISC.Models.UtilsProject;
using TestDISC.Queries.Interfaces;
using TestDISC.Services.Interfaces;

namespace TestDISC.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthQuery _authQuery;
        private readonly IAuthAction _authAction;
        private readonly JWT _jwt;

        public AuthService(IAuthQuery authQuery,
            IAuthAction authAction,
            IOptions<JWT> jwt)
        {
            _authQuery = authQuery;
            _authAction = authAction;
            _jwt = jwt.Value;
        }

        public Loginuser Login(LoginModel login, bool isEnscrypt = true)
        {
            if(isEnscrypt)
            {
                login.Password = Encryptor.MD5Hash(login.Password);
            }
            
            return _authQuery.Login(login);
        }

        public Loginuser GetUserById(ulong id)
        {
            return _authQuery.QueryUserById(id);
        }

        public JwtSecurityToken GenerateAccessToken(Loginuser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(Utils.ClaimPartnerId, user.Partnerid.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.Password),
                new Claim(Utils.ClaimUId, user.Id.ToString()),
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public void SetUserToSessionCookie(HttpResponse response, ISession session, string token, Loginuser loginuser, string refreshToken)
        {
            session.SetObjectAsJson(Utils.NameSession, loginuser);

            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(365)
            };

            response.Cookies.Append(Utils.NameCookie, token, options);
            response.Cookies.Append(Utils.NameRefreshCookie, refreshToken, options);
        }

        public (ulong?, JwtSecurityToken) ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwt.Key);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = Convert.ToUInt64(jwtToken.Claims.First(x => x.Type == Utils.ClaimUId).Value);

                // return account id from JWT token if validation successful
                return (accountId, jwtToken);
            }
            catch
            {
                // return null if validation fails
                return (null, null);
            }
        }

        public async Task<Refreshtoken> CreateRefreshToken(Loginuser user, string IPAddress, string token)
        {
            return await _authAction.CreateRefreshToken(user, IPAddress, token);
        }

        public async Task<(string, Refreshtoken)> UseRefreshToken(RefreshTokenModel refreshToken)
        {
            var entity = _authQuery.QueryRefreshTokenByValue(refreshToken.RefreshToken, refreshToken.AccessToken);

            if (entity is null || entity.Status != 10 || entity.Expiration <= DateTime.UtcNow)
            {
                return ($"Refresh token không hợp lệ.", null);
            }

            if (entity.Used == 1)
            {
                var otherRefresh = _authQuery.QueryRefreshTokenByUser(entity.Userid ?? 0);
                await _authAction.UpdateUsedOtherRefreshToken(otherRefresh);

                return ($"Refresh token đã được sử dụng.", null);
            }

            entity = await _authAction.UpdateUsedRefreshToken(entity);

            return ("", entity);
        }
    }
}