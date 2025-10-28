using System;
using System.Text.Json.Serialization;

namespace TestDISC.Models.Auth
{
    public class RefreshTokenModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        [JsonIgnore]
        public string IPAddress { get; set; }
    }
}

