using System;
namespace TestDISC.MServices.Interfaces
{
    public interface ISeleniumService
    {
        void SeleniumScreenShot(string webUrl, string path, int width = 480, int height = 760);
    }
}

