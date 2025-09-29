using NUnit.Framework;
using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.Pages;
using ProjectMars_AdvanceTask_NUnit.Pages.Components;
using ProjectMars_AdvanceTask_NUnit.Pages.Dashboard;
using ProjectMars_AdvanceTask_NUnit.Pages.ManageRequests;
using ProjectMars_AdvanceTask_NUnit.Pages.Notification;
using ProjectMars_AdvanceTask_NUnit.Pages.SearchSkill;
using ProjectMars_AdvanceTask_NUnit.Pages.Shared;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace ProjectMars_AdvanceTask_NUnit.Steps
{
   
    public class NotificationSteps
    {
        private readonly IWebDriver _driver;
        private readonly TestStateInfo _state; 
        private readonly MainNavigationComponent _mainNavigationComponent;
        private readonly NavigationHelper _navigationPage;
        private readonly SignInPage _signInPage;
        private readonly SearchSkillPage _searchSkillPage;
        private readonly RequestSkillTradePage _requestPage;
        private readonly NotificationComponent _notification;
        private readonly NotificationPage _notificationPage;
        private readonly SentRequestPage _sentRequestPage;
        private readonly ShareSkillSteps _shareSkillStep;
        public NotificationSteps(IWebDriver driver,TestStateInfo state)
        {
            _driver = driver;
            _state = state;
            _mainNavigationComponent = new MainNavigationComponent(_driver);
            _navigationPage = new NavigationHelper(_driver);
            _signInPage = new SignInPage(_driver);
            _searchSkillPage= new SearchSkillPage(_driver,_state);
            _requestPage = new RequestSkillTradePage(_driver);
            _notification = new NotificationComponent(_driver);
            _notificationPage= new NotificationPage(_driver);
            _sentRequestPage = new SentRequestPage(_driver);
            _shareSkillStep = new ShareSkillSteps (_driver, _state);    
        }



        public void UserASendsSkillRequest(string email, string password, string title,string message, int hours)
        {
          
            SignIn(email, password);
            SearchAndSelectSkill(title); 
            _requestPage.SendRequest(message, hours);

            _mainNavigationComponent.SignOut();
        }

        public void UserBSubmitsShareSkill(string email, string password, string dataKey)
        {

            if (_mainNavigationComponent.IsSignOutVisible()) _mainNavigationComponent.SignOut();

            SignIn(email, password);    
            _shareSkillStep.GivenIHaveShareSkillData(dataKey);
            _shareSkillStep.WhenISubmitTheShareSkillRecord();

            _mainNavigationComponent.SignOut();
        }

        public void UserBVerifiesNotificationAndMarksAsRead(string email, string password, string requestMessage)
        {
            SignIn(email,password);

            VerifyAndSelectNotification(requestMessage);                       
            
            _notificationPage.ClickMarkAsRead();
        }

        public void UserBVerifiesNotificationAndDelete(string email, string password, string requestMessage)
        {
            SignIn(email, password);

            VerifyAndSelectNotification(requestMessage);

            _notificationPage.ClickDelete();
        }

        public void CleanupNotificationByMessage(string requestMessage)
        {
            if (_notificationPage.ClickNotificationCheckboxByMessage(requestMessage))
            {
              _notificationPage.ClickDelete();
            }

            else
            {
                Console.WriteLine($"No notification message found for {requestMessage}"); 
            }

                _mainNavigationComponent.SignOut();
        }

        public void WithdrawServiceRequest(string email, string password, string title, string requestMessage)
        {
            _navigationPage.NavigateTo("");
            _navigationPage.GoToSignInPage();

            _signInPage.DoSignIn(email, password);

            _mainNavigationComponent.GoToManageRequests("Sent");
            _sentRequestPage.WithdrawRequest(requestMessage);

            _mainNavigationComponent.SignOut();

        }
     

        //Helper methods  

        private void SignIn(string email, string password)
        {
            _navigationPage.NavigateTo("");
            _navigationPage.GoToSignInPage();
            _signInPage.DoSignIn(email, password);
        }

        private void SearchAndSelectSkill(string title)
        {
            _mainNavigationComponent.ClickSearchSkill();
            _searchSkillPage.ClickCardByTitle(title);

        }
 
        private void VerifyAndSelectNotification(string expectedMessage)
        {

            if (!_notification.IsNotificationPresent(expectedMessage))
                throw new Exception("Notification not found!");

            _mainNavigationComponent.GoToDashboard();
            _notificationPage.ClickFirstNotificationCheckbox();
        }

        // Private utility method to centralize fetching of notification data from deserialization
        private NotificationData GetNotificationData(string dataKey)
        {
            var data = TestDataHelper.GetNotificationData(dataKey);

            if (data == null)
                throw new Exception($"Notification data for key '{dataKey}' is null.");

            _state.CurrentNotification = data;
            Console.WriteLine($"Loaded Notification Data: {data.Notification.Header}, {data.Notification.Content}");

            return data;
        }


    }

}
