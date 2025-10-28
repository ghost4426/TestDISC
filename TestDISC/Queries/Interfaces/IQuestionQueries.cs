using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestDISC.Models.Question;

namespace TestDISC.Queries.Interfaces
{
    public interface IQuestionQueries
    {
        Task<IList<QuestionDetailModel>> QueryQuestionDetail(ulong questionId);
    }
}
