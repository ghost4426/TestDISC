using System;
using System.Threading.Tasks;
using TestDISC.Actions.Interfaces;
using TestDISC.Database;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.LogAction;

namespace TestDISC.Actions
{
    public class LogActionAction : ILogActionAction
    {
        private readonly TestDISCContext _testDISCContext;

        public LogActionAction(TestDISCContext testDISCContext)
        {
            _testDISCContext = testDISCContext;
        }

        public async Task Create(LogActionModel logAction)
        {
            var logaction = new Logaction
            {
                Value = logAction.Value,
                Action = logAction.Action,
                Userid = logAction.UserId,
                Ipaddress = logAction.IPAddress,
                Createdate = DateTime.Now,
            };

            _testDISCContext.Add(logaction);
            await _testDISCContext.SaveChangesAsync();
        }
    }
}

