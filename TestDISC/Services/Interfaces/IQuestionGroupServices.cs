using System;
using System.Threading.Tasks;
using TestDISC.Models.QuestionGroup;

namespace TestDISC.Services.Interfaces
{
    public interface IQuestionGroupServices
    {
        Task<QuestionGroupModel> GetQuestionGroup();
    }
}
