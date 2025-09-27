using NUnit.Framework;
using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Hooks;
using ProjectMars_AdvanceTask_NUnit.Pages;
using ProjectMars_AdvanceTask_NUnit.Steps;
using ProjectMars_AdvanceTask_NUnit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Tests
{
    public class SignInTest : TestBase
    {
        private SignInSteps _signInSteps;
        private SignInPage _signInPage;

        [SetUp]
        public void Setup()
        {
            _signInSteps = new SignInSteps(Driver);
            _signInPage = new SignInPage(Driver);
        }

        [Test, Category("SignIn")]
        public void givenValidSignIn_whenDoSignIn_thenSignedIn()
        {
            var user = TestDataHelper.GetUserData("TestUser");
            _signInSteps.doSignIn(user.Email, user.Password);

             
            Assert.That(user.Greeting, Is.EqualTo(_signInPage.GetGreetingText()), "Greeting message is not as expected.");

        }
    }

}
