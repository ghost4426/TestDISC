using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.Auth;
using TestDISC.Models.InfoTest;
using TestDISC.Models.Report;
using TestDISC.Models.ResultDISC;
using TestDISC.Models.User;
using TestDISC.Models.UtilsProject;
using TestDISC.Models.UtilsProject.Filters;
using TestDISC.Services.Interfaces;

namespace TestDISC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInfoTestServices _infoTestServices;
        private readonly IResultDISCServices _resultDISCServices;
        private readonly IUserServices _userService;
        private readonly IAuthService _authService;
        private readonly ISession _session;

        public HomeController(IInfoTestServices infoTestServices,
            IResultDISCServices resultDISCServices,
            IUserServices userService,
            IAuthService authService,
            IHttpContextAccessor httpContextAccessor)
        {
            _infoTestServices = infoTestServices;
            _resultDISCServices = resultDISCServices;
            _userService = userService;
            _authService = authService;
            _session = httpContextAccessor.HttpContext.Session;
        }

        //Trang home, lấy danh sách câu hỏi
        public async Task<IActionResult> IndexAsync(ulong partnerid = 1)
        {
            var infoTest = await _infoTestServices.GetInfoTest();
            infoTest.UserCreate.partnerid = partnerid;

            return View(infoTest);
        }

        //Nút "kết quả"
        [HttpPost]
        public async Task<IActionResult> IndexAsync(InfoTestModel infoTest)
        {
            var dateNow = Utils.DateNow();
            //infoTest.UserCreate.partnerid = Utils.TopSkillId;

            //Lưu câu hỏi
            var result = await _resultDISCServices.SaveUserAnswer(dateNow, infoTest);

            if(result.Item2.Trim().Length == 0)
            {
                //Lưu thành công thì update kết quả
                var resultDISC = await _resultDISCServices.UpdateResultDISCInUserAnswer(result.Item3.Id);
                //Send email
                _resultDISCServices.SendEmailResult(result.Item1.UserCreate, resultDISC, result.Item3);

                return RedirectToAction("Result", new { useranswerid = result.Item3.Id });
            }

            ViewBag.Message = result.Item2;

            return View(infoTest);
        }

        public async Task<IActionResult> SendEmail(ulong useranswerid, int typegraph = 1)
        {
            //Lấy kết quả DISC của bài test
            var resultDISC = await _resultDISCServices.GetResult(useranswerid, typegraph);
            var userCreate = await _userService.GetUserInfo(new UserFilter
            {
                useranswerid = useranswerid,
            });

            if (resultDISC == null)
            {
                return RedirectToAction("ErrorResult");
            }

            //Send email
            _resultDISCServices.SendEmailResult(userCreate, resultDISC, new Useranswer
            {
                Id = useranswerid,
            });

            return View((resultDISC, userCreate));
        }

        public async Task<IActionResult> ResultAsync(ulong useranswerid, int typegraph = 1)
        {
            //Lấy kết quả DISC của bài test
            var resultDISC = await _resultDISCServices.GetResult(useranswerid, typegraph);
            var userCreate = await _userService.GetUserInfo(new UserFilter
            {
                useranswerid = useranswerid,
            });

            if (resultDISC == null)
            {
                return RedirectToAction("ErrorResult");
            }

            return View((resultDISC, userCreate));
        }

        public async Task<IActionResult> ResultV2Async(ulong useranswerid, int typegraph = 2)
        {
            //Lấy kết quả DISC của bài test
            var resultDISC = await _resultDISCServices.GetResult(useranswerid, typegraph);
            var userCreate = await _userService.GetUserInfo(new UserFilter
            {
                useranswerid = useranswerid,
            });

            if (resultDISC == null)
            {
                return RedirectToAction("ErrorResult");
            }

            return View((resultDISC, userCreate));
        }

        public IActionResult MostGraph(string point)
        {
            if (string.IsNullOrWhiteSpace(point))
            {
                return RedirectToAction("ErrorResult");
            }

            var data = JsonConvert.DeserializeObject<IList<PointAnswerModel>>(point);

            return View(data);
        }

        public IActionResult MostGraphV2(string point)
        {
            if (string.IsNullOrWhiteSpace(point))
            {
                return RedirectToAction("ErrorResult");
            }

            var data = JsonConvert.DeserializeObject<IList<PointAnswerModel>>(point);

            return View(data);
        }

        public IActionResult LeastGraph(string point)
        {
            if (string.IsNullOrWhiteSpace(point))
            {
                return RedirectToAction("ErrorResult");
            }

            var data = JsonConvert.DeserializeObject<IList<PointAnswerModel>>(point);

            return View(data);
        }

        public IActionResult LeastGraphV2(string point)
        {
            if (string.IsNullOrWhiteSpace(point))
            {
                return RedirectToAction("ErrorResult");
            }

            var data = JsonConvert.DeserializeObject<IList<PointAnswerModel>>(point);

            return View(data);
        }

        public async Task<IActionResult> FormPDF(ulong useranswerid, int typegraph = 1)
        {
            //Vẽ trang HTML PDF để in ra
            var resultDISC = await _resultDISCServices.GetResult(useranswerid, typegraph);
            var userCreate = await _userService.GetUserInfo(new UserFilter
            {
                useranswerid = useranswerid,
            });

            if (resultDISC == null)
            {
                return RedirectToAction("ErrorResult");
            }

            return View((resultDISC, userCreate));
        }

        public IActionResult ErrorResult()
        {
            return View();
        }

        [TypeFilter(typeof(AdminFilter))]
        public IActionResult Download()
        {
            return View();
        }

        [TypeFilter(typeof(AdminFilter))]
        public async Task<IActionResult> ExportReport()
        {
            var dateNow = Utils.DateNow();
            var resutlDISCExcel = await _resultDISCServices.ExportExcel(dateNow, new ReportFilter
            {
                partnerviewid = Utils.TopSkillId,
            });

            return File(resutlDISCExcel.Stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", resutlDISCExcel.FileName);
        }

        [TypeFilter(typeof(LoginFilter))]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(LoginFilter))]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _authService.Login(login);

            if (user == null)
            {
                ModelState.AddModelError("", "Tài khoản của bạn không tồn tại.");
                return View(login);
            }

            var jwtSecurityToken = _authService.GenerateAccessToken(user);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshTokenEntity = await _authService.CreateRefreshToken(user, DetectIpAddress(), token);

            _authService.SetUserToSessionCookie(Response, _session, token, user, refreshTokenEntity.Value);

            return RedirectToAction("", "Control", new { Area = "" });
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
