using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDISC.Models.Question;
using TestDISC.Models.QuestionGroup;
using TestDISC.MServices.Interfaces;
using TestDISC.Queries.Interfaces;

namespace TestDISC.Queries
{
    public class QuestionGroupQueries : IQuestionGroupQueries
    {
        private readonly ITestDISCDapper _testDISCDapper;
        private readonly IQuestionQueries _questionQueries;

        public QuestionGroupQueries(ITestDISCDapper testDISCDapper,
            IQuestionQueries questionQueries)
        {
            _testDISCDapper = testDISCDapper;
            _questionQueries = questionQueries;
        }

        public async Task<QuestionGroupModel> QueryQuestionGroup()
        {
            var query =
                @"select qg.id, qg.title, ifnull(qg.description, '') description, ifnull(qg.suggest, '') suggest, 
	                ifnull(qg.tutorial, '') tutorial 
                from questiongroup qg 
                where qg.status = 1 and id = 1 ";

            var questionGroup = await _testDISCDapper.QuerySingleAsync<QuestionGroupModel>(query);

            if(questionGroup != null)
            {
                questionGroup.Questions = await QueryQuestionGroupDetail(questionGroup.id);
            }

            return questionGroup;
        }

        public async Task<IList<QuestionModel>> QueryQuestionGroupDetail(ulong questionGroupId)
        {
            var query =
                @"select q.id, q.content, 0 mostchoosenid, 0 leastchoosenid 
                from question q 
	                inner join questiongroupdetail qgd on qgd.questionid = q.id 
                where qgd.status = 1 and q.status = 1 and qgd.questiongroupid = @questionGroupId 
                order by qgd.orderview asc ";

            var questions = (await _testDISCDapper.QueryAsync<QuestionModel>(query, new
            {
                questionGroupId
            })).ToList();

            if (questions != null && questions.Count > 0)
            {
                foreach(var quesItem in questions)
                {
                    quesItem.QuestionDetails = await _questionQueries.QueryQuestionDetail(quesItem.id);
                }
            }

            return questions;
        }
    }
}
