using System;
using System.Collections.Generic;

namespace TestDISC.Models.ResultDISC
{
    public class ResultDISCModel
    {
        public ulong id { get; set; }
        public string code { get; set; }
        public string codetext { get; set; }
        public string description { get; set; }
        public string interpret { get; set; }
        public string quanlyti { get; set; }
        public ulong useranswerid { get; set; }
        public string linkmost { get; set; }
        public string linkleast { get; set; }
        public IList<PointAnswerModel> pointMosts { get; set; }
        public IList<PointAnswerModel> pointLeasts { get; set; }
    }
}
