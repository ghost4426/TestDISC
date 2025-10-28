using System;
using System.Collections.Generic;
using System.Linq;
using TestDISC.Database;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.Auth;
using TestDISC.Models.UtilsProject;
using TestDISC.Queries.Interfaces;

namespace TestDISC.Queries
{
    public class AuthQuery : IAuthQuery
    {
        private readonly TestDISCContext _testDISCContext;

        public AuthQuery(TestDISCContext testDISCContext)
        {
            _testDISCContext = testDISCContext;
        }

        public Loginuser Login(LoginModel login)
        {
            var loginuser = _testDISCContext.Loginuser.Where(a =>
                    a.Status == 1 &&
                    a.Email.Trim().Equals(login.Email.Trim()) &&
                    a.Password.Equals(login.Password.Trim()))
                .FirstOrDefault();

            return loginuser;
        }

        public Loginuser QueryUserById(ulong id)
        {
            var loginuser = _testDISCContext.Loginuser.Where(a => a.Status == 1 && a.Id == id).FirstOrDefault();

            return loginuser;
        }

        public Refreshtoken QueryRefreshTokenByValue(string refreshToken, string token)
        {
            return _testDISCContext.Refreshtoken.Where(a => a.Value == refreshToken && a.Accesstoken == token).FirstOrDefault();
        }

        public List<Refreshtoken> QueryRefreshTokenByUser(ulong userId)
        {
            return _testDISCContext.Refreshtoken.Where(a => a.Status == 10 && a.Used == 0 && a.Userid == userId).ToList();
        }
    }
}

