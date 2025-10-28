using System;
using System.Threading.Tasks;
using TestDISC.Models.QuestionGroup;
using TestDISC.Queries.Interfaces;
using TestDISC.Services.Interfaces;

namespace TestDISC.Services
{
    public class QuestionGroupServices : IQuestionGroupServices
    {
        private readonly IQuestionGroupQueries _questionGroupQueries;

        public QuestionGroupServices(IQuestionGroupQueries questionGroupQueries)
        {
            _questionGroupQueries = questionGroupQueries;
        }

        public async Task<QuestionGroupModel> GetQuestionGroup()
        {
            return await _questionGroupQueries.QueryQuestionGroup();
        }
    }
}
