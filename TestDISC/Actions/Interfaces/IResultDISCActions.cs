using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.Question;
using TestDISC.Models.QuestionGroup;
using TestDISC.Models.ResultDISC;

namespace TestDISC.Actions.Interfaces
{
    public interface IResultDISCActions
    {
        Task<Useranswer> CreateUserAnswer(DateTime datenow, User user, QuestionGroupModel questionGroup);
        Task<IList<Useranswerquestion>> CreateUserAnswerQuestionList(Useranswer useranswer, IList<QuestionModel> questions);
        Task<IList<Useranswerquestiondetail>> CreateUserAnswerQuestionDetailList(IList<Useranswerquestion> useranswerquestions, IList<QuestionModel> questions);
        Task UpdateResultDISCInAnswer(ulong useranswerId, ResultDISCModel resultDISC);
    }
}
