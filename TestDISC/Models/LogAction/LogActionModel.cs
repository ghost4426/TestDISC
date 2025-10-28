using System;
namespace TestDISC.Models.LogAction
{
    public class LogActionModel
    {
        public string Value { get; set; }
        public string Action { get; set; }
        public ulong UserId { get; set; }
        public string IPAddress { get; set; }
    }
}

