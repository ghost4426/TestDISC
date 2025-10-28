using System;
using System.Text.Json.Serialization;
using TestDISC.Models.QuestionGroup;
using TestDISC.Models.User;

namespace TestDISC.Models.InfoTest
{
    public class InfoTestModel
    {
        public UserCreate UserCreate { get; set; }
        public QuestionGroupModel QuestionGroup { get; set; }
    }
}
