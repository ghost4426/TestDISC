using System;
using System.Drawing;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestDISC.Models.UtilsProject;
using TestDISC.MServices.Interfaces;

namespace TestDISC.MServices
{
    public class SeleniumService : ISeleniumService
    {
        private IWebDriver _driver;
        private readonly ChromeSetting _chromeSetting;

        public SeleniumService(IOptions<ChromeSetting> chromeSetting)
        {
            _chromeSetting = chromeSetting.Value;
        }

        public void SeleniumScreenShot(string webUrl, string name, int width = 480, int height = 760)
        {
            if (!Directory.Exists(Utils.SavePath))
            {
                Directory.CreateDirectory(Utils.SavePath);
            }

            if(string.IsNullOrEmpty(_chromeSetting.Path))
            {
                _driver = new ChromeDriver();
            }
            else
            {
                _driver = new ChromeDriver(_chromeSetting.Path);
            }
            
            _driver.Manage().Window.Size = new System.Drawing.Size(width, height);
            _driver.Navigate().GoToUrl(webUrl);
            try
            {
                System.Threading.Thread.Sleep(1000);
                Screenshot TakeScreenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                TakeScreenshot.SaveAsFile(Path.Combine(Utils.SavePath, name));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            _driver.Quit();
        }
    }
}

