using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDISC.Models.Question;
using TestDISC.MServices.Interfaces;
using TestDISC.Queries.Interfaces;

namespace TestDISC.Queries
{
    public class QuestionQueries : IQuestionQueries
    {
        private readonly ITestDISCDapper _testDISCDapper;

        public QuestionQueries(ITestDISCDapper testDISCDapper)
        {
            _testDISCDapper = testDISCDapper;
        }

        public async Task<IList<QuestionDetailModel>> QueryQuestionDetail(ulong questionId)
        {
            var query =
                @"select id, content, mostpoint, leastpoint 
                from questiondetail qd 
                where qd.status = 1 and qd.questionid = @questionId 
                order by qd.orderview asc ";

            return (await _testDISCDapper.QueryAsync<QuestionDetailModel>(query, new
            {
                questionId
            })).ToList();
        }
    }
}
