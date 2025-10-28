using System;
using System.Threading.Tasks;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.User;

namespace TestDISC.Actions.Interfaces
{
    public interface IUserActions
    {
        Task<User> Create(DateTime dateNow, UserCreate userCreate);
    }
}
