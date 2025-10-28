using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TestDISC.Models.Question;

namespace TestDISC.Models.QuestionGroup
{
    public class QuestionGroupModel
    {
        public ulong id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string tutorial { get; set; }
        public string suggest { get; set; }
        public int ActiveQuestion { get; set; }
        public IList<QuestionModel> Questions { get; set; }
    }
}
