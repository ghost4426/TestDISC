using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.InfoTest;
using TestDISC.Models.Report;
using TestDISC.Models.ResultDISC;

namespace TestDISC.Queries.Interfaces
{
    public interface IResultDISCQueries
    {
        Task<ResultDISCModel> QueryResultDISC(OSearchResultDISC oSearchResultDISC);
        Task<IList<ResultDISCReportModel>> QueryReport(ReportFilter filter);
        Task<IList<PointAnswerModel>> QueryMostPoint(ulong useranswerid);
        Task<IList<PointAnswerModel>> QueryLeastPoint(ulong useranswerid);
        Dictionary<string, string> GetMaxLeastPoint(IList<PointAnswerModel> leastList);
        (InfoTestModel, string, Useranswer) CheckValidAnswer(InfoTestModel infoTest);
        void ExportReportSheets(ExcelPackage package, IList<ResultDISCReportModel> resultDISCReports);
    }
}
