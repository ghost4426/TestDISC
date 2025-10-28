using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestDISC.Database.TestDISCModels
{
    public partial class User
    {
        public ulong Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? Status { get; set; }
        public string Fullname { get; set; }
        public DateTime? Createdate { get; set; }
        public DateTime? Updatedate { get; set; }
        public string Namecompany { get; set; }
        public ulong? Title { get; set; }
        public string Jobposition { get; set; }
        public ulong? Partnerid { get; set; }
    }
}
