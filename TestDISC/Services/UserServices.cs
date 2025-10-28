using System;
using System.Threading.Tasks;
using TestDISC.Actions.Interfaces;
using TestDISC.Models.User;
using TestDISC.Queries.Interfaces;
using TestDISC.Services.Interfaces;

namespace TestDISC.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserActions _userActions;
        private readonly IUserQuery _userQuery;

        public UserServices(IUserActions userActions,
            IUserQuery userQuery)
        {
            _userActions = userActions;
            _userQuery = userQuery;
        }

        public async Task<UserCreate> GetUserInfo(UserFilter filter)
        {
            return await _userQuery.QueryUserInfo(filter);
        }
    }
}
