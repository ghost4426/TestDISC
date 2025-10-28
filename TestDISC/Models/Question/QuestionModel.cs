using System;
using System.Collections.Generic;

namespace TestDISC.Models.Question
{
    public class QuestionModel
    {
        public ulong id { get; set; }
        public string content { get; set; }
        public ulong mostchoosenid { get; set; }
        public ulong leastchoosenid { get; set; }
        public IList<QuestionDetailModel> QuestionDetails { get; set; }
    }
}
