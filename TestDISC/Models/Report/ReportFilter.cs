using System;
namespace TestDISC.Models.Report
{
    public class ReportFilter
    {
        public ulong useranswerid { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public ulong partnerid { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public ulong partnerviewid { get; set; }
        public int currentpage { get; set; }
        public int pagesize { get; set; }
    }
}