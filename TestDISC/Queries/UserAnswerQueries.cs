using System;
using System.Threading.Tasks;
using TestDISC.Models.UserAnswer;
using TestDISC.MServices.Interfaces;
using TestDISC.Queries.Interfaces;

namespace TestDISC.Queries
{
    public class UserAnswerQueries : IUserAnswerQueries
    {
        private readonly ITestDISCDapper _testDISCDapper;

        public UserAnswerQueries(ITestDISCDapper testDISCDapper)
        {
            _testDISCDapper = testDISCDapper;
        }

        public async Task<UserAnswerModel> QueryUserAnswer(ulong userAnswerId)
        {
            var query =
                @"select id, ifnull(resultdiscid, 0) resultdiscid 
                from useranswer
                where status = 1 and id = @userAnswerId";

            return await _testDISCDapper.QuerySingleAsync<UserAnswerModel>(query, new
            {
                userAnswerId,
            });
        }

        public async Task<bool> CheckViewAnswer(ulong userAnswerId, ulong partnerId)
        {
            var query =
                @"select id, ifnull(resultdiscid, 0) resultdiscid 
                from useranswer
                where status = 1 and id = @userAnswerId and partnerid = @partnerId ";

            var userAnswer = await _testDISCDapper.QuerySingleAsync<UserAnswerModel>(query, new
            {
                userAnswerId,
                partnerId,
            });

            return userAnswer != null;
        }
    }
}
