using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestDISC.Database.TestDISCModels
{
    public partial class Useranswerquestiondetail
    {
        public ulong Id { get; set; }
        public ulong? Questionid { get; set; }
        public ulong? Mostquestiondetailid { get; set; }
        public ulong? Leastquestiondetailid { get; set; }
        public ulong? Useranswerquestionid { get; set; }
    }
}
