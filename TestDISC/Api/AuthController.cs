using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestDISC.Models.Auth;
using TestDISC.Models.UtilsProject;
using TestDISC.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestDISC.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if(string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            {
                return BadRequest(new ObjectResponse<object>
                {
                    Success = false,
                    Message = "Vui lòng điền email và mật khẩu."
                });
            }

            var user = _authService.Login(login);

            if(user == null)
            {
                return BadRequest(new ObjectResponse<object>
                {
                    Success = false,
                    Message = "Tài khoản của bạn không tồn tại."
                });
            }

            var jwtSecurityToken = _authService.GenerateAccessToken(user);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshtoken = await _authService.CreateRefreshToken(user, DetectIpAddress(), token);

            var authentication = new AuthenticationModel
            {
                email = user.Email,
                partnerid = user.Partnerid ?? 0,
                token = token,
                refreshToken = refreshtoken.Value,
            };

            return Ok(new ObjectResponse<AuthenticationModel>
            {
                Success = true,
                Data = authentication,
            });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshToken(RefreshTokenModel refreshToken)
        {
            refreshToken.IPAddress = DetectIpAddress();

            var result = await _authService.UseRefreshToken(refreshToken);

            if(!string.IsNullOrWhiteSpace(result.Item1))
            {
                return BadRequest(new ObjectResponse<string>
                {
                    Success = false,
                    Message = result.Item1,
                });
            }

            var user = _authService.GetUserById(result.Item2.Userid ?? 0);

            if (user is null)
            {
                return BadRequest(new ObjectResponse<string>
                {
                    Success = false,
                    Message = $"Tài khoản không tồn tại.",
                });
            }

            var jwtSecurityToken = _authService.GenerateAccessToken(user);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var newRefreshToken = await _authService.CreateRefreshToken(user, refreshToken.IPAddress, token);

            return Ok(new ObjectResponse<AuthenticationModel>
            {
                Success = true,
                Data = new AuthenticationModel
                {
                    email = user.Email,
                    partnerid = user.Partnerid ?? 0,
                    token = token,
                    refreshToken = newRefreshToken.Value,
                }
            });
        }

        private string DetectIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}

