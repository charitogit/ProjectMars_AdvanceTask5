using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;
using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Hooks;
using ProjectMars_AdvanceTask_NUnit.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjectMars_AdvanceTask_NUnit.Steps
{
    public class SignInSteps
    {
        private readonly NavigationHelper _navigationPage;
        private readonly SignInPage _signInPage;
        public SignInSteps(IWebDriver driver)
        {
            _signInPage = new SignInPage(driver);
            _navigationPage = new NavigationHelper(driver);
        }

        public void doSignIn(string email, string password)
        {
            _navigationPage.NavigateTo("");
            _navigationPage.GoToSignInPage();

            _signInPage.RenderComponents();
           _signInPage.DoSignIn(email,password);

        }


    }
}
