using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDISC.Models.User;
using TestDISC.MServices.Interfaces;
using TestDISC.Queries.Interfaces;

namespace TestDISC.Queries
{
    public class UserQuery : IUserQuery
    {
        private readonly ITestDISCDapper _testDISCDapper;

        public UserQuery(ITestDISCDapper testDISCDapper)
        {
            _testDISCDapper = testDISCDapper;
        }

        public async Task<UserCreate> QueryUserInfo(UserFilter filter)
        {
            var condition = "";

            if(filter.id > 0)
            {
                condition += @" and u.id = @id ";
            }

            if(filter.useranswerid > 0)
            {
                condition += 
                    @" and u.id in ( 
                        select userid 
                        from useranswer 
                        where id = @useranswerid 
                    ) ";
            }

            var query =
                @"select u.email, u.phone, u.fullname, ifnull(u.namecompany, '') namecompany, ifnull(u.title, 0) titleid,
                    ifnull(u.jobposition, '') jobposition, p.name partnername  
                from user u
                    inner join partner p on p.id = u.partnerid 
                where u.status = 1 " + condition + @" ";

            return await _testDISCDapper.QuerySingleAsync<UserCreate>(query, new
            {
                filter.id,
                filter.useranswerid,
            });
        }
    }
}
