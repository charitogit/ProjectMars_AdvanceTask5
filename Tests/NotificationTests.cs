using NUnit.Framework;
using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Hooks;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.Pages.Dashboard;
using ProjectMars_AdvanceTask_NUnit.Pages.SearchSkill;
using ProjectMars_AdvanceTask_NUnit.Pages.Shared;
using ProjectMars_AdvanceTask_NUnit.Steps;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Tests
{
    [TestFixture]
    public class NotificationTests : TestBase
    {

        private ShareSkillSteps _shareSkillSteps;
        private NotificationSteps _notificationSteps;
        private NotificationPage _notificationPage;
        private MainNavigationComponent _mainNavigationComponent;


        [SetUp]
        public void Setup()
        {
            _mainNavigationComponent = new MainNavigationComponent(Driver); 
            _notificationSteps = new NotificationSteps(Driver,State);
            _notificationPage = new NotificationPage(Driver); 
            _shareSkillSteps = new ShareSkillSteps(Driver, State);
        }

        [Test]
        [Category("Notification")]
        [Category("ShareSkill")]
        public void Notification_ShouldAppearAndBeMarkedAsRead()
        {
            var data = TestDataHelper.GetNotificationData("MarkAsReadNotification");

            //  Phase 1: User B creates Share Skill 
           _notificationSteps.UserBSubmitsShareSkill(data.UserB.Email, data.UserB.Password, "validShareSkill3");

            //  Phase 2: User A requests the Shared Skill by User B 
           _notificationSteps.UserASendsSkillRequest(data.UserA.Email, data.UserA.Password, data.Request.Title, data.Request.Message, data.Request.Hours);
            
            //  Phase 3: User B checks notification and marks as Read   
            _notificationSteps.UserBVerifiesNotificationAndMarksAsRead(data.UserB.Email, data.UserB.Password, data.Notification.Content);
           
            //Assert
            Assert.That(_notificationPage.IsNotificationMarkedAsRead(data.Notification.Content), Is.True, "Notification is still bold after marking as read.");

            //  Post Cleanup 
            _notificationSteps.CleanupNotificationByMessage(data.Notification.Content);
            _notificationSteps.WithdrawServiceRequest(data.UserA.Email, data.UserA.Password, data.Request.Title, data.Request.Message);
            
            }

        [Test]
        [Category("Notification")]
        [Category("ShareSkill")]
        public void Notification_ShouldAppearAndBeDeleted()
        {
            var data = TestDataHelper.GetNotificationData("DeleteNotification");

            //  Phase 1: User B creates Share Skill 
            _notificationSteps.UserBSubmitsShareSkill(data.UserB.Email, data.UserB.Password, "validShareSkill3");

            //  Phase 2: User A requests the skill 
            _notificationSteps.UserASendsSkillRequest(data.UserA.Email, data.UserA.Password, data.Request.Title, data.Request.Message, data.Request.Hours);
            Thread.Sleep(2000);

            //  Phase 3: User B checks notification and perform Delete 
            _notificationSteps.UserBVerifiesNotificationAndDelete(data.UserB.Email, data.UserB.Password, data.Notification.Content);
              
            
            //Assert
             AssertionHelper.AssertToastMessage(_notificationPage.GetActualMessage(), data.Notification.PopUpMessage);
            
            //  Post Cleanup 
            _notificationSteps.CleanupNotificationByMessage(data.Notification.Content);
            _notificationSteps.WithdrawServiceRequest(data.UserA.Email, data.UserA.Password, data.Request.Title, data.Request.Message);
           
        }
    }
}