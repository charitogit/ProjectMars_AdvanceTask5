using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Pages.Notification
{
    public class NotificationComponent
    {
        private readonly IWebDriver _driver;

        private readonly By notificationDropdown = By.XPath("//div[@class='ui top left pointing dropdown item']");
        private readonly By markAllAsReadLink = By.XPath("//div[@class='ui link item']//a[text()='Mark all as read']");
        private readonly By notificationItems = By.XPath("//div[@class='ui menu link transition visible']//div[contains(@class,'item')]//div[@class='content']//a");

        public NotificationComponent(IWebDriver driver)
        {
            _driver = driver;
        }

        public void OpenNotificationDropdown() => _driver.FindElement(notificationDropdown).Click();

        public void ClickMarkAllAsRead()
        {
            OpenNotificationDropdown();
            _driver.FindElement(markAllAsReadLink).Click();
        }

        public bool IsNotificationPresent(string expectedMessage)
        {
            OpenNotificationDropdown();
            return _driver.FindElements(notificationItems)
                         .Any(n => n.Text.Contains(expectedMessage));
        }

        public bool AreAllNotificationsNonBold()
        {
            OpenNotificationDropdown();
            var notifications = _driver.FindElements(notificationItems);

            return notifications.All(n =>
            {
                string fontWeight = n.GetCssValue("font-weight");
                return fontWeight != "700" && !fontWeight.Equals("bold", StringComparison.OrdinalIgnoreCase);
            });
        }
    }

}
