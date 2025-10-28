using System;
using System.Threading.Tasks;
using TestDISC.Models.InfoTest;
using TestDISC.Models.User;
using TestDISC.Queries.Interfaces;
using TestDISC.Services.Interfaces;

namespace TestDISC.Services
{
    public class InfoTestServices : IInfoTestServices
    {
        private readonly IQuestionGroupQueries _questionGroupQueries;

        public InfoTestServices(IQuestionGroupQueries questionGroupQueries)
        {
            _questionGroupQueries = questionGroupQueries;
        }

        public async Task<InfoTestModel> GetInfoTest()
        {
            //Lấy danh sách câu hỏi
            var questionGroup = await _questionGroupQueries.QueryQuestionGroup();
            ////Câu hỏi bắt đầu, 0: câu đầu tiên
            questionGroup.ActiveQuestion = 0;

            return new InfoTestModel
            {
                UserCreate = new UserCreate(),
                QuestionGroup = questionGroup,
            };
        }
    }
}
