using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMars_AdvanceTask_NUnit.Utilities;

namespace ProjectMars_AdvanceTask_NUnit.Pages
{
    public class SignInPage
    {

        private readonly IWebDriver _driver;

        private IWebElement emailInput;
        private IWebElement passwordInput;
        private IWebElement loginButton;

        private IWebElement signoutButton; 

        public SignInPage(IWebDriver driver)
        {
            _driver = driver;
        }
       
        public void RenderComponents()
        {
            try
            {
                string emailInputXPath= "//input[@name='email']";
                string passwordInputXPath = "//input[@name='password']";
                string loginButtonXPath= "//button[contains(text(),'Login')]";

                Wait.WaitToBeVisible(_driver, "XPath", emailInputXPath, 10);
                Wait.WaitToBeVisible(_driver, "XPath", passwordInputXPath, 10);
                Wait.WaitToBeVisible(_driver, "XPath", loginButtonXPath, 10);

                emailInput = _driver.FindElement(By.XPath(emailInputXPath));
                passwordInput = _driver.FindElement(By.XPath(passwordInputXPath));
                loginButton = _driver.FindElement(By.XPath(loginButtonXPath));
            }
            catch (Exception ex)
            {
                Console.WriteLine("[RenderComponents] Error rendering elements: " + ex.Message);
                throw;
            }
        }


        public void DoSignIn(string email, string password)
        {
            RenderComponents();

            // enter email
            emailInput.Clear();
            emailInput.SendKeys(email);

            // enter password  
            passwordInput.Clear();
            passwordInput.SendKeys(password);

            // click login button
            loginButton.Click();

        }

       public void SignOut()
        {
            string signoutButtonXPath = "//button[text()='Sign Out']";
            Wait.WaitToBeVisible(_driver, "XPath", signoutButtonXPath, 10);
            signoutButton = _driver.FindElement(By.XPath(signoutButtonXPath));
            signoutButton.Click();
        }

        public string GetGreetingText()
        {
            Wait.WaitToBeVisible(_driver, "XPath", "//div[@id='account-profile-section']//span[contains(text(), 'Hi')]\r\n", 10);
            string greetingText = _driver.FindElement(By.XPath("//div[@id='account-profile-section']//span[contains(text(), 'Hi')]\r\n")).Text;
            return greetingText;
        }

    
        public bool IsSignInFormVisible()
        {
            return emailInput.Displayed && passwordInput.Displayed;
        }




    }
}
