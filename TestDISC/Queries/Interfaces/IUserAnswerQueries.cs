using System;
using System.Threading.Tasks;
using TestDISC.Models.UserAnswer;

namespace TestDISC.Queries.Interfaces
{
    public interface IUserAnswerQueries
    {
        Task<UserAnswerModel> QueryUserAnswer(ulong userAnswerId);
        Task<bool> CheckViewAnswer(ulong userAnswerId, ulong partnerId);
    }
}
