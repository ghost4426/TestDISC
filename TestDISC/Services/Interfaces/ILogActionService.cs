using System;
using System.Threading.Tasks;
using TestDISC.Models.LogAction;

namespace TestDISC.Services.Interfaces
{
    public interface ILogActionService
    {
        Task CreateLogAction(LogActionModel logAction);
    }
}

