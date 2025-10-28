using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestDISC.Database.TestDISCModels;

namespace TestDISC.Actions.Interfaces
{
    public interface IAuthAction
    {
        Task<Refreshtoken> CreateRefreshToken(Loginuser user, string IPAddress, string token);
        Task UpdateUsedOtherRefreshToken(List<Refreshtoken> refreshtokens);
        Task<Refreshtoken> UpdateUsedRefreshToken(Refreshtoken refreshtoken);
    }
}

