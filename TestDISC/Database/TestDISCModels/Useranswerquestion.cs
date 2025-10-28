using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestDISC.Database.TestDISCModels
{
    public partial class Useranswerquestion
    {
        public ulong Id { get; set; }
        public ulong? Useranswerid { get; set; }
        public ulong? Questionid { get; set; }
    }
}
