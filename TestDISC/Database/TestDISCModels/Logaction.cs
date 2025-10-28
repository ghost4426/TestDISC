using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestDISC.Database.TestDISCModels
{
    public partial class Logaction
    {
        public ulong Id { get; set; }
        public string Value { get; set; }
        public string Action { get; set; }
        public ulong? Userid { get; set; }
        public string Ipaddress { get; set; }
        public DateTime? Createdate { get; set; }
    }
}
