using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestDISC.Database.TestDISCModels
{
    public partial class Questiondetail
    {
        public ulong Id { get; set; }
        public ulong? Questionid { get; set; }
        public string Content { get; set; }
        public int? Orderview { get; set; }
        public int? Mostpoint { get; set; }
        public int? Leastpoint { get; set; }
        public int? Status { get; set; }
    }
}
