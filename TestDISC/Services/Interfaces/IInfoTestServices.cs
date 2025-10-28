using System;
using System.Threading.Tasks;
using TestDISC.Models.InfoTest;

namespace TestDISC.Services.Interfaces
{
    public interface IInfoTestServices
    {
        Task<InfoTestModel> GetInfoTest();
    }
}
