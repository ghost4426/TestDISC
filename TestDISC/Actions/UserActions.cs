using System;
using System.Linq;
using System.Threading.Tasks;
using TestDISC.Actions.Interfaces;
using TestDISC.Database;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.User;

namespace TestDISC.Actions
{
    public class UserActions : IUserActions
    {
        private readonly TestDISCContext _testDISCContext;

        public UserActions(TestDISCContext testDISCContext)
        {
            _testDISCContext = testDISCContext;
        }

        public async Task<User> Create(DateTime dateNow, UserCreate userCreate)
        {
            var user = _testDISCContext.User.Where(a =>
                a.Email.Equals(userCreate.email) &&
                a.Phone.Equals(userCreate.phone) &&
                a.Partnerid == userCreate.partnerid).FirstOrDefault();

            if(user == null)
            {
                user = new User
                {
                    Fullname = userCreate.fullname,
                    Email = userCreate.email,
                    Phone = userCreate.phone,
                    Namecompany = userCreate.namecompany,
                    Title = userCreate.titleid,
                    Partnerid = userCreate.partnerid,
                    Jobposition = userCreate.jobposition,
                    Status = 1,
                    Createdate = dateNow,
                    Updatedate = dateNow,
                };

                _testDISCContext.User.Add(user);
                await _testDISCContext.SaveChangesAsync();
            }
            else
            {
                user.Fullname = userCreate.fullname;
                user.Email = userCreate.email;
                user.Phone = userCreate.phone;
                user.Namecompany = userCreate.namecompany;
                user.Title = userCreate.titleid;
                user.Partnerid = userCreate.partnerid;
                user.Jobposition = userCreate.jobposition;
                user.Status = 1;
                user.Updatedate = dateNow;

                _testDISCContext.User.Update(user);
                await _testDISCContext.SaveChangesAsync();
            }

            return user;
        }
    }
}
