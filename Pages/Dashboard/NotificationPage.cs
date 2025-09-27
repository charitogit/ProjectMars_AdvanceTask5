using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Pages.Dashboard
{
    public class NotificationPage
    {
        private readonly IWebDriver _driver;

        public NotificationPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // ===== XPath locators =====
        private string selectAllXPath = "//div[@data-tooltip='Select all']";
        private string unselectAllXPath = "//div[@data-tooltip='Unselect all']";
        private string deleteXPath = "//div[@data-tooltip='Delete selection']";
        private string markAsReadXPath = "//div[@data-tooltip='Mark selection as read']";
        private string loadMoreXPath = "//a[contains(text(),'Load More')]";
        private string showLessXPath = "//a[contains(text(),'Show Less')]";
        private readonly string firstNotificationCheckboxXPath = "(//div[@class='item link']//input[@type='checkbox'])[1]";

        private readonly string deleteMessageXpath = "//div[contains(@class,'ns-box-inner')]";

        // ========== Element Fields ==========
        public IWebElement SelectAllButton => _driver.FindElement(By.XPath(selectAllXPath));        
        public IWebElement UnselectAllButton => _driver.FindElement(By.XPath(unselectAllXPath));
        public IWebElement DeleteButton => _driver.FindElement(By.XPath(deleteXPath));  
        public IWebElement MarkAsReadButton => _driver.FindElement(By.XPath(markAsReadXPath));     
        public IWebElement ShowLessButton => _driver.FindElement(By.XPath(showLessXPath));
        public IWebElement LoadMoreButton => _driver.FindElement(By.XPath(loadMoreXPath));
        public IWebElement FirstNotificationCheckbox => _driver.FindElement(By.XPath(firstNotificationCheckboxXPath));


        // ===== Actions =====
        public void ClickSelectAll()
        {
            Wait.WaitToBeClickable(_driver, "XPath", selectAllXPath, 10);
            SelectAllButton.Click();

        }

        public void ClickUnselectAll()
        {
             Wait.WaitToBeClickable(_driver, "XPath", unselectAllXPath, 10);
            UnselectAllButton.Click();
        }

        public void ClickDelete()
        {

                Wait.WaitToBeClickable(_driver, "XPath", deleteXPath, 10);
                DeleteButton.Click();

                // Handle the toast pop-up automatically
                try
                {
                    string toastXPath = "//div[contains(@class,'ns-box ns-growl ns-effect-jelly')]";
                    Wait.WaitToBeVisible(_driver, "XPath", toastXPath, 5);

                    // Find close button inside the toast
                    string closeButtonXPath = toastXPath + "//a[@class='ns-close']";
                    var closeButton = _driver.FindElement(By.XPath(closeButtonXPath));

                    closeButton.Click();
                    Console.WriteLine("Notification toast closed after delete.");
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("No notification toast appeared after delete.");
                }
           
        }

        public void ClickMarkAsRead()
        {
           Wait.WaitToBeClickable(_driver, "XPath", markAsReadXPath, 10);
            MarkAsReadButton.Click();
        }

        public void ClickLoadMore()
        {
            var elements = _driver.FindElements(By.XPath(loadMoreXPath));
            if (elements.Count > 0)
            {
                Wait.WaitToBeClickable(_driver, "XPath", loadMoreXPath, 10);
                LoadMoreButton.Click();
            }
        }

        public void ClickShowLess()
        {
            var elements = _driver.FindElements(By.XPath(showLessXPath));
            if (elements.Count > 0)
            {
                Wait.WaitToBeClickable(_driver, "XPath", showLessXPath, 10);
                ShowLessButton.Click();
            }
        }

    
        public void ClickFirstNotificationCheckbox()
        {
            Wait.WaitToBeClickable(_driver, "XPath", firstNotificationCheckboxXPath, 10);
            FirstNotificationCheckbox.Click();
        }

        public bool ClickNotificationCheckboxByMessage(string messageText)
        {
            try
            {
                // 1. Find the notification row that contains the message
                string rowXPath = $"//div[contains(@class,'sixteen wide column row')]" +
                                  $"[.//div[@class='content' and contains(text(),'{messageText}')]]";

                var notificationRow = _driver.FindElement(By.XPath(rowXPath));

                // 2. Find the checkbox inside that row
                var checkbox = notificationRow.FindElement(By.XPath(".//input[@type='checkbox']"));

                // 3. Click the checkbox
                checkbox.Click();

                Console.WriteLine($"Clicked checkbox for notification: {messageText}");

                return true;
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Notification not found: {messageText}");
                return false;
            }
            catch (StaleElementReferenceException)
            {
                Console.WriteLine($"[WARN] Notification element went stale while clicking: {messageText}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Unexpected error clicking checkbox: {ex.Message}");
                return false;
            }
        }


        public void DeleteAllNotifications()
        {
            ClickSelectAll();
            ClickDelete();
        }

      

        // ========== Assertions ==========
        public bool IsNotificationMarkedAsRead(string messageText)
        {
            string xpath = $"//div[@class='item link']//div[@class='content' and contains(.,'{messageText}')]";
            Wait.WaitToBeVisible(_driver, "XPath", xpath, 10);

            IWebElement notificationContent = _driver.FindElement(By.XPath(xpath));

            // Assumption: bold = <b> or <strong> or CSS class
            // Here we check if no <b> or <strong> exists inside
            bool hasBold = notificationContent.FindElements(By.XPath(".//b | .//strong")).Count > 0;

            return !hasBold; // true if no bold → notification is read
        }

       public string GetActualMessage()
        {
            Wait.WaitToBeVisible(_driver, "XPath", deleteMessageXpath, 10);
            IWebElement deleteMessage = _driver.FindElement(By.XPath(deleteMessageXpath));
            string actualMessage = deleteMessage.Text.Trim();
            
            return actualMessage;
        }
    }
}
