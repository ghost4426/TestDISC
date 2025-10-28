using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TestDISC.Actions.Interfaces;
using TestDISC.Database;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.InfoTest;
using TestDISC.Models.Report;
using TestDISC.Models.ResultDISC;
using TestDISC.Models.User;
using TestDISC.Models.UtilsProject;
using TestDISC.MServices;
using TestDISC.MServices.Interfaces;
using TestDISC.Queries.Interfaces;
using TestDISC.Services.Interfaces;

namespace TestDISC.Services
{
    public class ResultDISCServices : IResultDISCServices
    {
        private readonly IUserActions _userActions;
        private readonly IResultDISCActions _resultDISCActions;
        private readonly IResultDISCQueries _resultDISCQueries;
        private readonly IUserAnswerQueries _userAnswerQueries;
        private readonly IEmailService _emailService;
        private readonly TestDISCContext _testDISCContext;
        private readonly IWebHostEnvironment _env;
        private readonly LinkBase _linkBase;
        private readonly ISeleniumService _seleniumService;

        public ResultDISCServices(IUserActions userActions,
            IResultDISCActions resultDISCActions,
            IResultDISCQueries resultDISCQueries,
            IUserAnswerQueries userAnswerQueries,
            IEmailService emailService,
            TestDISCContext testDISCContext,
            IWebHostEnvironment env,
            IOptions<LinkBase> linkBase,
            ISeleniumService seleniumService)
        {
            _userActions = userActions;
            _resultDISCActions = resultDISCActions;
            _resultDISCQueries = resultDISCQueries;
            _userAnswerQueries = userAnswerQueries;
            _emailService = emailService;
            _testDISCContext = testDISCContext;
            _env = env;
            _linkBase = linkBase.Value;
            _seleniumService = seleniumService;
        }

        public async Task<(InfoTestModel, string, Useranswer)> SaveUserAnswer(DateTime dateNow, InfoTestModel infoTest)
        {
            //Check các câu trả lời có hợp với điều kiện hay không
            var error = _resultDISCQueries.CheckValidAnswer(infoTest);

            if(error.Item2.Trim().Length > 0)
            {
                return error;
            }

            //Tạo user khi tham gia trả lời
            var user = await _userActions.Create(dateNow, infoTest.UserCreate);

            //Mở transaction
            var tran = await _testDISCContext.Database.BeginTransactionAsync();

            try
            {
                //Lưu bộ câu hỏi cho user
                var useranswer = await _resultDISCActions.CreateUserAnswer(dateNow, user, infoTest.QuestionGroup);
                //Lưu danh sách câu hỏi
                var useranswerquestions = await _resultDISCActions.CreateUserAnswerQuestionList(useranswer, infoTest.QuestionGroup.Questions);
                //Lưu câu trả lời tương ứng mới câu hỏi
                await _resultDISCActions.CreateUserAnswerQuestionDetailList(useranswerquestions, infoTest.QuestionGroup.Questions);

                await tran.CommitAsync();

                return (infoTest, "", useranswer);
            }
            catch(Exception)
            {
                await tran.RollbackAsync();
                return (infoTest, "Đã có lỗi xảy ra. Vui lòng thử lại.", null);
            }
        }

        public async Task<ResultDISCModel> UpdateResultDISCInUserAnswer(ulong userAnswerId)
        {
            //Tính ra kết quả LEAST
            var codePairs = _resultDISCQueries.GetMaxLeastPoint(await _resultDISCQueries.QueryLeastPoint(userAnswerId));

            //Tìm kết quả DISC tương ứng
            var resultDISC = await _resultDISCQueries.QueryResultDISC(new OSearchResultDISC
            {
                code = int.Parse(codePairs["code"]),
                codeText = codePairs["codeText"],
            });

            //Update vào thông tin kết quả của user
            await _resultDISCActions.UpdateResultDISCInAnswer(userAnswerId, resultDISC);

            return resultDISC;
        }

        public async Task<ResultDISCModel> GetResult(ulong useranswerid, int typegraph)
        {
            //Lấy bài test
            var taskUserAnswer = _userAnswerQueries.QueryUserAnswer(useranswerid);
            //Lấy câu MOST
            var taskMostList = _resultDISCQueries.QueryMostPoint(useranswerid);
            //Lấy câu LEAST
            var taskLeastList = _resultDISCQueries.QueryLeastPoint(useranswerid);

            await Task.WhenAll(taskMostList, taskLeastList, taskUserAnswer);

            ResultDISCModel resultDISC;

            if (taskUserAnswer.Result != null && taskUserAnswer.Result.resultdiscid != 0)
            {
                //Có lưu trong kết quả thì lấy kết quả DISC tương ứng
                resultDISC = await _resultDISCQueries.QueryResultDISC(new OSearchResultDISC
                {
                    id = taskUserAnswer.Result.resultdiscid,
                });
            }
            else
            {
                if(taskUserAnswer.Result != null)
                {
                    //Không có thì tính lại kết quả
                    var codePairs = _resultDISCQueries.GetMaxLeastPoint(taskLeastList.Result);

                    resultDISC = await _resultDISCQueries.QueryResultDISC(new OSearchResultDISC
                    {
                        code = int.Parse(codePairs["code"]),
                        codeText = codePairs["codeText"],
                    });
                }
                else
                {
                    return null;
                }
            }

            resultDISC.useranswerid = useranswerid;
            resultDISC.pointMosts = taskMostList.Result;
            resultDISC.pointLeasts = taskLeastList.Result;
            resultDISC.linkmost = $"{Utils.BuildResultLink(_linkBase.SelfLink)}{useranswerid}-most-result-{typegraph}.png";
            resultDISC.linkleast = $"{Utils.BuildResultLink(_linkBase.SelfLink)}{useranswerid}-least-result-{typegraph}.png";

            return resultDISC;
        }

        public async Task<ResultDISCModel> GetOnlyResult(ulong useranswerid, int typegraph)
        {
            //Lấy bài test
            var taskUserAnswer = _userAnswerQueries.QueryUserAnswer(useranswerid);
            //Lấy câu MOST
            var taskMostList = _resultDISCQueries.QueryMostPoint(useranswerid);
            //Lấy câu LEAST
            var taskLeastList = _resultDISCQueries.QueryLeastPoint(useranswerid);

            await Task.WhenAll(taskMostList, taskLeastList, taskUserAnswer);

            var resultDISC = await _resultDISCQueries.QueryResultDISC(new OSearchResultDISC
            {
                id = taskUserAnswer.Result.resultdiscid,
            });

            resultDISC.useranswerid = useranswerid;
            resultDISC.pointMosts = taskMostList.Result;
            resultDISC.pointLeasts = taskLeastList.Result;
            resultDISC.linkmost = $"{Utils.BuildResultLink(_linkBase.SelfLink)}{useranswerid}-most-result-{typegraph}.png";
            resultDISC.linkleast = $"{Utils.BuildResultLink(_linkBase.SelfLink)}{useranswerid}-least-result-{typegraph}.png";

            return resultDISC;
        }

        public bool ExistResultImage(ulong useranswerid, int typegraph)
        {
            return File.Exists(Path.Combine(Utils.SavePath, $"{useranswerid}-most-result-{typegraph}.png"));
        }

        public void GenerateResultImage(ResultDISCModel resultDISC, ulong useranswerid, int typegraph)
        {
            if(typegraph == 2)
            {
                var pointMosts = JsonConvert.SerializeObject(resultDISC.pointMosts);
                _seleniumService.SeleniumScreenShot($"{_linkBase.MostV2Url}?point={pointMosts}", $"{useranswerid}-most-result-2.png", 530, 765);

                var pointLeasts = JsonConvert.SerializeObject(resultDISC.pointLeasts);
                _seleniumService.SeleniumScreenShot($"{_linkBase.LeastV2Url}?point={pointLeasts}", $"{useranswerid}-least-result-2.png", 530, 765);
            }
            else
            {
                var pointMosts = JsonConvert.SerializeObject(resultDISC.pointMosts);
                _seleniumService.SeleniumScreenShot($"{_linkBase.MostUrl}?point={pointMosts}", $"{useranswerid}-most-result-1.png");

                var pointLeasts = JsonConvert.SerializeObject(resultDISC.pointLeasts);
                _seleniumService.SeleniumScreenShot($"{_linkBase.LeastUrl}?point={pointLeasts}", $"{useranswerid}-least-result-1.png");
            }
        }

        public async Task<bool> CheckViewAnswer(ulong userAnswerId, ulong partnerId)
        {
            return await _userAnswerQueries.CheckViewAnswer(userAnswerId, partnerId);
        }

        public void SendEmailResult(UserCreate user, ResultDISCModel resultDISC, Useranswer useranswer)
        {
            var absoPath = Path.Combine("wwwroot", "Assets", "Template", "Result", "ResultTemplateV2.html");
            var pathTemp = Path.Combine(_env.ContentRootPath, absoPath);

            var content = File.ReadAllText(pathTemp);
            content = content.Replace("{{titleuser}}", Utils.ConvertTitleUserVN(user.titleid));
            content = content.Replace("{{titleuserVN}}", Utils.ConvertTitleUserVN(user.titleid));
            content = content.Replace("{{username}}", user.fullname);
            content = content.Replace("{{linkuseranswer}}", Utils.BuildLinkUserAnswer(_linkBase.SelfLink, useranswer.Id));
            //content = content.Replace("{{email}}", user.email);
            //content = content.Replace("{{resultdisc}}", resultDISC.codetext);
            //content = content.Replace("{{interpret}}", resultDISC.interpret);
            //content = content.Replace("{{quanlyti}}", resultDISC.quanlyti);
            //content = content.Replace("{{description}}", resultDISC.description);

            var subject = "TIÊU ĐỀ [TOPSKILLS] BÁO CÁO KẾT QUẢ TEST DISC RÚT GỌN - " + Utils.ConvertTitleUserVN(user.titleid) + " " + user.fullname;

            _emailService.Send(user.email, subject, content);
        }

        public async Task<IList<ResultDISCReportModel>> GetReport(ReportFilter filter)
        {
            return await _resultDISCQueries.QueryReport(filter);
        }

        public async Task<ResutlDISCExcelModel> ExportExcel(DateTime dateNow, ReportFilter filter)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var stream = new MemoryStream();
            var filename = "Excel kết quả DISC (" + dateNow.ToString("yyyy-MM-dd HH:mm:ss") + ")" + ".xlsx";
            var resultDISCReports = await _resultDISCQueries.QueryReport(filter);

            using (var package = new ExcelPackage(stream))
            {
                _resultDISCQueries.ExportReportSheets(package, resultDISCReports);

                package.Save();
            }

            stream.Position = 0;

            return new ResutlDISCExcelModel
            {
                Stream = stream,
                FileName = filename,
            };
        }
    }
}
