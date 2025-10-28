using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestDISC.Database.TestDISCModels
{
    public partial class Useranswer
    {
        public ulong Id { get; set; }
        public ulong? Userid { get; set; }
        public ulong? Questiongroupid { get; set; }
        public int? Status { get; set; }
        public DateTime? Createdate { get; set; }
        public ulong? Resultdiscid { get; set; }
        public ulong? Partnerid { get; set; }
    }
}
