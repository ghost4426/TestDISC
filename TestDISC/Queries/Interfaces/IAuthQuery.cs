using System;
using System.Collections.Generic;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.Auth;

namespace TestDISC.Queries.Interfaces
{
    public interface IAuthQuery
    {
        Loginuser Login(LoginModel login);
        Loginuser QueryUserById(ulong id);
        Refreshtoken QueryRefreshTokenByValue(string refreshToken, string token);
        List<Refreshtoken> QueryRefreshTokenByUser(ulong userId);
    }
}

