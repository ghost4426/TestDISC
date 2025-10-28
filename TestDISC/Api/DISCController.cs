using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.InfoTest;
using TestDISC.Models.LogAction;
using TestDISC.Models.Report;
using TestDISC.Models.ResultDISC;
using TestDISC.Models.User;
using TestDISC.Models.UtilsProject;
using TestDISC.MServices;
using TestDISC.MServices.Interfaces;
using TestDISC.Services;
using TestDISC.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestDISC.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DISCController : Controller
    {
        private readonly IInfoTestServices _infoTestServices;
        private readonly IResultDISCServices _resultDISCServices;
        private readonly IUserServices _userServices;
        private readonly ILogActionService _logActionService;
        private readonly ISeleniumService _seleniumService;
        private readonly LinkBase _linkBase;

        public DISCController(IInfoTestServices infoTestServices,
            IResultDISCServices resultDISCServices,
            IUserServices userServices,
            ILogActionService logActionService,
            ISeleniumService seleniumService,
            IOptions<LinkBase> linkBase)
        {
            _infoTestServices = infoTestServices;
            _resultDISCServices = resultDISCServices;
            _userServices = userServices;
            _logActionService = logActionService;
            _seleniumService = seleniumService;
            _linkBase = linkBase.Value;
        }

        [HttpGet("questions")]
        public async Task<IActionResult> GetInfoTest()
        {
            var userId = Convert.ToUInt64(HttpContext.User.FindFirst(Utils.ClaimUId).Value);
            var infoTest = await _infoTestServices.GetInfoTest();

            await _logActionService.CreateLogAction(new LogActionModel
            {
                UserId = userId,
                Action = "Lấy bộ thông tin và câu hỏi",
                IPAddress = DetectIpAddress()
            });

            return Ok(new ObjectResponse<InfoTestModel>
            {
                Success = true,
                Data = infoTest,
            });
        }

        [HttpPost("save-answer")]
        public async Task<IActionResult> SaveAnswer(InfoTestModel infoTest, int typegraph = 1)
        {
            var userId = Convert.ToUInt64(HttpContext.User.FindFirst(Utils.ClaimUId).Value);

            //Kiểm tra thông tin form và nội dung câu trả lời
            var message = ValidateSaveAnswer(infoTest);

            if(!string.IsNullOrWhiteSpace(message))
            {
                return BadRequest(new ObjectResponse<string>
                {
                    Success = false,
                    Message = message,
                });
            }

            var dateNow = Utils.DateNow();
            var partnerId = Convert.ToUInt64(HttpContext.User.FindFirst(Utils.ClaimPartnerId).Value);
            infoTest.UserCreate.partnerid = partnerId;

            //Lưu câu hỏi
            var result = await _resultDISCServices.SaveUserAnswer(dateNow, infoTest);

            if (result.Item2.Trim().Length > 0)
            {
                return Ok(new ObjectResponse<string>
                {
                    Success = true,
                    Data = result.Item2,
                });
            }

            //Lưu thành công thì update kết quả
            await _resultDISCServices.UpdateResultDISCInUserAnswer(result.Item3.Id);
            //Send email
            //_resultDISCServices.SendEmailResult(result.Item1.UserCreate, resultDISC, result.Item3);

            var resultDISC = await _resultDISCServices.GetResult(result.Item3.Id, typegraph);
            _resultDISCServices.GenerateResultImage(resultDISC, result.Item3.Id, typegraph);

            await _logActionService.CreateLogAction(new LogActionModel
            {
                Value = JsonConvert.SerializeObject(infoTest),
                UserId = userId,
                Action = "Lưu thông tin và câu trả lời",
                IPAddress = DetectIpAddress()
            });

            return Ok(new ObjectResponse<ulong>
            {
                Success = true,
                Data = result.Item3.Id,
            });
        }

        [HttpGet("{useranswerid}/result")]
        public async Task<IActionResult> GetResult(ulong useranswerid, int typegraph = 1)
        {
            //Kiểm tra id hợp lệ
            if(useranswerid < 1)
            {
                return BadRequest(new ObjectResponse<string>
                {
                    Success = false,
                    Message = "Vui lòng chọn kết quả để xem.",
                });
            }

            //Kiểm tra xem user được xem kết quả hay không
            var userId = Convert.ToUInt64(HttpContext.User.FindFirst(Utils.ClaimUId).Value);
            var partnerId = Convert.ToUInt64(HttpContext.User.FindFirst(Utils.ClaimPartnerId).Value);

            if(!Utils.isTopSkills(partnerId))
            {
                var available = await _resultDISCServices.CheckViewAnswer(useranswerid, partnerId);

                if (available == false)
                {
                    return BadRequest(new ObjectResponse<string>
                    {
                        Success = false,
                        Message = "Không có kết quả tương ứng.",
                    });
                }
            }

            ResultDISCModel resultDISC;

            //Kiểm tra xem hình hết quả có không
            if (_resultDISCServices.ExistResultImage(useranswerid, typegraph))
            {
                //Có thì lấy kết quả và thêm hình
                resultDISC = await _resultDISCServices.GetOnlyResult(useranswerid, typegraph);
            }
            else
            {
                // Không có thì tạo hình ảnh
                resultDISC = await _resultDISCServices.GetResult(useranswerid, typegraph);
                _resultDISCServices.GenerateResultImage(resultDISC, useranswerid, typegraph);
            }

            //resultDISC.pointMosts = null;
            //resultDISC.pointLeasts = null;

            var userCreate = await _userServices.GetUserInfo(new UserFilter
            {
                useranswerid = useranswerid,
            });

            if (resultDISC == null)
            {
                return BadRequest(new ObjectResponse<string>
                {
                    Success = false,
                    Message = "Không có kết quả tương ứng.",
                });
            }

            return Ok(new ObjectResponse<object>
            {
                Success = true,
                Data = new
                {
                    ResultDISC = resultDISC,
                    UserCreate = userCreate,
                },
            });
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReport(ulong useranswerid = 0, string fromDate = "", string toDate = "",
            ulong partnerid = 0, string email = "", string phone = "", int currentpage = 0, int pagesize = 20)
        {
            var userId = Convert.ToUInt64(HttpContext.User.FindFirst(Utils.ClaimUId).Value);
            var partnerViewId = Convert.ToUInt64(HttpContext.User.FindFirst(Utils.ClaimPartnerId).Value);

            var filter = new ReportFilter
            {
                useranswerid = useranswerid,
                fromDate = fromDate,
                toDate = toDate,
                partnerid = partnerid,
                email = email,
                phone = phone,
                partnerviewid = partnerViewId,
                currentpage = currentpage,
                pagesize = pagesize,
            };

            var data = await _resultDISCServices.GetReport(filter);

            await _logActionService.CreateLogAction(new LogActionModel
            {
                Value = JsonConvert.SerializeObject(filter),
                UserId = userId,
                Action = "Lấy danh sách kết quả",
                IPAddress = DetectIpAddress()
            });

            return Ok(new ObjectResponse<IList<ResultDISCReportModel>>
            {
                Success = true,
                Data = data,
            });
        }

        private string ValidateSaveAnswer(InfoTestModel infoTest)
        {
            if (infoTest.UserCreate == null)
            {
                return "Vui lòng điền đầy đủ thông tin đăng ký.";
            }

            if (string.IsNullOrWhiteSpace(infoTest.UserCreate.fullname))
            {
                return "Vui lòng điền họ tên.";
            }

            if (string.IsNullOrWhiteSpace(infoTest.UserCreate.email))
            {
                return "Vui lòng điền email.";
            }

            if (string.IsNullOrWhiteSpace(infoTest.UserCreate.phone))
            {
                return "Vui lòng điền số điện thoại.";
            }

            //if (string.IsNullOrWhiteSpace(infoTest.UserCreate.jobposition))
            //{
            //    return "Vui lòng điền vị trí công việc.";
            //}

            if(infoTest.QuestionGroup == null)
            {
                return "Vui lòng điền câu trả lời.";
            }

            if (infoTest.QuestionGroup.id < 1)
            {
                return "Vui lòng chọn nhóm câu hỏi.";
            }

            return "";
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

