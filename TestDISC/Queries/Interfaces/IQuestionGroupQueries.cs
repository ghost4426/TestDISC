using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestDISC.Models.Question;
using TestDISC.Models.QuestionGroup;

namespace TestDISC.Queries.Interfaces
{
    public interface IQuestionGroupQueries
    {
        Task<QuestionGroupModel> QueryQuestionGroup();
        Task<IList<QuestionModel>> QueryQuestionGroupDetail(ulong questionGroupId);
    }
}
