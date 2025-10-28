using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.InfoTest;
using TestDISC.Models.Report;
using TestDISC.Models.ResultDISC;
using TestDISC.Models.User;

namespace TestDISC.Services.Interfaces
{
    public interface IResultDISCServices
    {
        Task<(InfoTestModel, string, Useranswer)> SaveUserAnswer(DateTime dateNow, InfoTestModel infoTest);
        Task<ResultDISCModel> UpdateResultDISCInUserAnswer(ulong userAnswerId);
        Task<ResultDISCModel> GetResult(ulong useranswerid, int typegraph);
        Task<ResultDISCModel> GetOnlyResult(ulong useranswerid, int typegraph);
        Task<bool> CheckViewAnswer(ulong userAnswerId, ulong partnerId);
        bool ExistResultImage(ulong useranswerid, int typegraph);
        void GenerateResultImage(ResultDISCModel resultDISC, ulong useranswerid, int typegraph);
        void SendEmailResult(UserCreate user, ResultDISCModel resultDISC, Useranswer useranswer);
        Task<IList<ResultDISCReportModel>> GetReport(ReportFilter filter);
        Task<ResutlDISCExcelModel> ExportExcel(DateTime dateNow, ReportFilter filter);
    }
}
