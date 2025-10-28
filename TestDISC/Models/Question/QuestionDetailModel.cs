using System;
using System.Text.Json.Serialization;

namespace TestDISC.Models.Question
{
    public class QuestionDetailModel
    {
        public ulong id { get; set; }
        public string content { get; set; }

        [JsonIgnore]
        public int mostpoint { get; set; }

        [JsonIgnore]
        public int leastpoint { get; set; }
    }
}
