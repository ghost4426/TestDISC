using System;
using System.Threading.Tasks;
using TestDISC.Models.User;

namespace TestDISC.Services.Interfaces
{
    public interface IUserServices
    {
        Task<UserCreate> GetUserInfo(UserFilter filter);
    }
}
