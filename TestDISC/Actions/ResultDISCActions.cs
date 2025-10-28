using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDISC.Actions.Interfaces;
using TestDISC.Database;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.Question;
using TestDISC.Models.QuestionGroup;
using TestDISC.Models.ResultDISC;

namespace TestDISC.Actions
{
    public class ResultDISCActions : IResultDISCActions
    {
        private readonly TestDISCContext _testDISCContext;

        public ResultDISCActions(TestDISCContext testDISCContext)
        {
            _testDISCContext = testDISCContext;
        }

        public async Task<Useranswer> CreateUserAnswer(DateTime datenow, User user, QuestionGroupModel questionGroup)
        {
            var useranswer = new Useranswer
            {
                Userid = user.Id,
                Questiongroupid = questionGroup.id,
                Partnerid = user.Partnerid,
                Status = 1,
                Createdate = datenow,
            };

            _testDISCContext.Useranswer.Add(useranswer);
            await _testDISCContext.SaveChangesAsync();

            return useranswer;
        }

        public async Task<IList<Useranswerquestion>> CreateUserAnswerQuestionList(Useranswer useranswer, IList<QuestionModel> questions)
        {
            if(questions != null && questions.Count > 0)
            {
                var useranswerquestions = questions.Select(quesItem => new Useranswerquestion
                {
                    Useranswerid = useranswer.Id,
                    Questionid = quesItem.id,
                });

                _testDISCContext.Useranswerquestion.AddRange(useranswerquestions);
                await _testDISCContext.SaveChangesAsync();

                useranswerquestions = _testDISCContext.Useranswerquestion.Where(a => a.Useranswerid == useranswer.Id);

                return useranswerquestions.ToList();
            }

            return null;
        }

        public async Task<IList<Useranswerquestiondetail>> CreateUserAnswerQuestionDetailList(IList<Useranswerquestion> useranswerquestions,
            IList<QuestionModel> questions)
        {
            if(questions != null && questions.Count > 0)
            {
                var useranswerquestiondetails = questions.Select(quesItem => new Useranswerquestiondetail
                {
                    Useranswerquestionid = useranswerquestions.Where(a => a.Questionid == quesItem.id).FirstOrDefault().Id,
                    Questionid = quesItem.id,
                    Mostquestiondetailid = quesItem.mostchoosenid,
                    Leastquestiondetailid = quesItem.leastchoosenid,
                });

                _testDISCContext.Useranswerquestiondetail.AddRange(useranswerquestiondetails);
                await _testDISCContext.SaveChangesAsync();

                return useranswerquestiondetails.ToList();
            }

            return null;
        }

        public async Task UpdateResultDISCInAnswer(ulong useranswerId, ResultDISCModel resultDISC)
        {
            var useranswer = _testDISCContext.Useranswer.Where(a => a.Id == useranswerId).FirstOrDefault();

            if(useranswer != null)
            {
                useranswer.Resultdiscid = resultDISC.id;

                _testDISCContext.Useranswer.Update(useranswer);
                await _testDISCContext.SaveChangesAsync();
            }
        }
    }
}
