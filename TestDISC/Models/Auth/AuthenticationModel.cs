using System;
using System.Collections.Generic;

namespace TestDISC.Models.Auth
{
    public class AuthenticationModel
    {
        public string email { get; set; }
        public ulong partnerid { get; set; }
        public string token { get; set; }
        public string refreshToken { get; set; }
    }
}

