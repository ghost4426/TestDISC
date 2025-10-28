using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestDISC.Actions.Interfaces;
using TestDISC.Database;
using TestDISC.Database.TestDISCModels;

namespace TestDISC.Actions
{
    public class AuthAction : IAuthAction
    {
        private readonly TestDISCContext _testDISCContext;

        public AuthAction(TestDISCContext testDISCContext)
        {
            _testDISCContext = testDISCContext;
        }

        public async Task<Refreshtoken> CreateRefreshToken(Loginuser user, string IPAddress, string token)
        {
            var dateNow = DateTime.Now;

            var refreshtoken = new Refreshtoken
            {
                Value = Guid.NewGuid().ToString("N"),
                Expiration = DateTime.UtcNow.AddDays(7),
                Accesstoken = token,
                Used = 0,
                Ipaddress = IPAddress,
                Userid = user.Id,
                Status = 10,
                Createdate = dateNow,
                Updatedate = dateNow,
            };

            _testDISCContext.Refreshtoken.Add(refreshtoken);
            await _testDISCContext.SaveChangesAsync();

            return refreshtoken;
        }

        public async Task UpdateUsedOtherRefreshToken(List<Refreshtoken> refreshtokens)
        {
            foreach (var rt in refreshtokens)
            {
                rt.Used = 1;
                rt.Status = 10;
                rt.Updatedate = DateTime.Now;

                _testDISCContext.Refreshtoken.Update(rt);
            }

            await _testDISCContext.SaveChangesAsync();
        }

        public async Task<Refreshtoken> UpdateUsedRefreshToken(Refreshtoken refreshtoken)
        {
            refreshtoken.Used = 1;
            refreshtoken.Updatedate = DateTime.Now;

            _testDISCContext.Refreshtoken.Update(refreshtoken);
            await _testDISCContext.SaveChangesAsync();

            return refreshtoken;
        }
    }
}

