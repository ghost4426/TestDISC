using System;
using System.Text.Json.Serialization;

namespace TestDISC.Models.User
{
    public class UserCreate
    {
        public ulong titleid { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string namecompany { get; set; }
        public string jobposition { get; set; }
        public string partnername { get; set; }

        [JsonIgnore]
        public ulong partnerid { get; set; }
    }
}
