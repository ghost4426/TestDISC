using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDISC.Models.User;

namespace TestDISC.Queries.Interfaces
{
    public interface IUserQuery
    {
        Task<UserCreate> QueryUserInfo(UserFilter filter);
    }
}
