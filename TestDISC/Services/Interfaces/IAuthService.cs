using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.Auth;

namespace TestDISC.Services.Interfaces
{
    public interface IAuthService
    {
        Loginuser Login(LoginModel login, bool isEnscrypt = true);
        Loginuser GetUserById(ulong id);
        JwtSecurityToken GenerateAccessToken(Loginuser user);
        void SetUserToSessionCookie(HttpResponse response, ISession session, string token, Loginuser loginuser, string refreshToken);
        (ulong?, JwtSecurityToken) ValidateJwtToken(string token);
        Task<Refreshtoken> CreateRefreshToken(Loginuser user, string IPAddress, string token);
        Task<(string, Refreshtoken)> UseRefreshToken(RefreshTokenModel refreshToken);
    }
}

