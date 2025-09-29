using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Pages
{
    public class RequestSkillTradePage
    {
        private readonly IWebDriver _driver;

        private readonly By messageBox = By.XPath("//textarea[@placeholder='I am interested in trading my cooking skills with your coding skills..']");
        private readonly By hoursInput = By.XPath("//input[@placeholder='Hours']");
        private readonly By requestButton = By.XPath("//div[contains(@class,'ui teal') and contains(@class,'button')]");

        public RequestSkillTradePage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void SendRequest(string message, int hours)
        {
            _driver.FindElement(messageBox).Clear();
            _driver.FindElement(messageBox).SendKeys(message);
                     
            _driver.FindElement(requestButton).Click();

            ClickYesOnPrompt();
        }

        public void ClickYesOnPrompt()
        {
           string yesButtonXPath = "//button[text()='Yes']";

           Wait.WaitToBeVisible(_driver, "XPath", yesButtonXPath, 10);
           IWebElement yesButton = _driver.FindElement(By.XPath(yesButtonXPath));
           yesButton.Click();
        }

              
    }

}
