using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using TestDISC.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using TestDISC.Models.Auth;
using System.Linq;
using TestDISC.Database.TestDISCModels;

namespace TestDISC.Models.UtilsProject.Filters
{
    public class LoginFilter : IAsyncActionFilter
    {
        public readonly IAuthService _authService;

        public LoginFilter(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            var _session = filterContext.HttpContext.Session;
            var _cookie = filterContext.HttpContext.Request.Cookies;

            //Kiểm tra torng session có đăng nhập thì vào trang Control
            if (_session != null && _session.GetString(Utils.NameSession) != null)
            {
                filterContext.Result = RediectToControl();
                return;
            }
            else
            {
                //Nếu session không có thì kiểm tra cookie
                if (_cookie.ContainsKey(Utils.NameCookie) == true)
                {
                    var token = _cookie[Utils.NameCookie];
                    var valid = _authService.ValidateJwtToken(token);

                    //Lấy token từ cookie kiểm tra valid
                    if (valid.Item1 != null && valid.Item1 > 0)
                    {
                        //Phân giải lấy thông tin từ token
                        var email = valid.Item2.Claims.First(e => e.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
                        var password = valid.Item2.Claims.First(e => e.Type.Equals(JwtRegisteredClaimNames.FamilyName)).Value;

                        var user = _authService.Login(new LoginModel
                        {
                            Email = email,
                            Password = password,
                        }, false);

                        //Nếu đúng là user đó thì login vào hệ thống
                        if (user != null)
                        {
                            _session.SetObjectAsJson(Utils.NameSession, user);

                            filterContext.Result = RediectToControl();
                            return;
                        }
                        else
                        {
                            var result = await CheckRefeshToken(filterContext, token);
                            //Nếu refresh token đúng thì vào hệ thống
                            if (result == true)
                            {
                                filterContext.Result = RediectToControl();
                                return;
                            }
                        }
                    }
                    else
                    {
                        var result = await CheckRefeshToken(filterContext, token);
                        //Nếu refresh token đúng thì vào hệ thống
                        if (result == true)
                        {
                            filterContext.Result = RediectToControl();
                            return;
                        }
                    }
                }
            }

            await next();
        }

        private RedirectToRouteResult RediectToControl()
        {
            return new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    {"area", ""},
                    {"controller", "Control"},
                    {"action", ""}
                }
            );
        }

        private async Task<bool> CheckRefeshToken(ActionExecutingContext filterContext, string token)
        {
            var _session = filterContext.HttpContext.Session;
            var _cookie = filterContext.HttpContext.Request.Cookies;

            if (_cookie.ContainsKey(Utils.NameRefreshCookie) == true)
            {
                var refreshToken = new RefreshTokenModel
                {
                    AccessToken = token,
                    RefreshToken = _cookie[Utils.NameRefreshCookie],
                };

                if (filterContext.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                    refreshToken.IPAddress = filterContext.HttpContext.Request.Headers["X-Forwarded-For"];
                else
                    refreshToken.IPAddress = filterContext.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

                var result = await _authService.UseRefreshToken(refreshToken);

                if (string.IsNullOrWhiteSpace(result.Item1))
                {
                    var userEntity = _authService.GetUserById(result.Item2.Userid ?? 0);
                    var jwtSecurityToken = _authService.GenerateAccessToken(userEntity);
                    var newToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    var refreshTokenEntity = await _authService.CreateRefreshToken(userEntity, refreshToken.IPAddress, newToken);

                    _authService.SetUserToSessionCookie(filterContext.HttpContext.Response,
                        _session, newToken, userEntity, refreshTokenEntity.Value);

                    return true;
                }
                //Không thì về đăng nhập
                else
                {
                    return false;
                }
            }
            //Không thì về đăng nhập
            else
            {
                return false;
            }
        }
    }
}

