using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestDISC.MServices.Interfaces
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html);
    }
}
