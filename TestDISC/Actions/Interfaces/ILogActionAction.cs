using System;
using System.Threading.Tasks;
using TestDISC.Models.LogAction;

namespace TestDISC.Actions.Interfaces
{
    public interface ILogActionAction
    {
        Task Create(LogActionModel logAction);
    }
}

