using System;
using System.Threading.Tasks;
using TestDISC.Actions.Interfaces;
using TestDISC.Models.LogAction;
using TestDISC.Services.Interfaces;

namespace TestDISC.Services
{
    public class LogActionService : ILogActionService
    {
        private readonly ILogActionAction _logActionAction;

        public LogActionService(ILogActionAction logActionAction)
        {
            _logActionAction = logActionAction;
        }

        public async Task CreateLogAction(LogActionModel logAction)
        {
            await _logActionAction.Create(logAction);
        }
    }
}

