using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ProjectMars_AdvanceTask_NUnit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ProjectMars_AdvanceTask_NUnit.Pages.Shared
{
    public class MainNavigationComponent
    {
        private readonly IWebDriver _driver;

        public MainNavigationComponent(IWebDriver driver)
        {
            _driver = driver;
        }

        // XPath locators for elements on the main navigation

        private readonly string signOutButtonXPath = "//button[normalize-space()='Sign Out']";
        private readonly string  searchSkillIconXPath = "//div[contains(@class, 'search-box')]//i[contains(@class, 'search') and contains(@class, 'link') and contains(@class, 'icon')]";
        private readonly string shareSkillXPath = "//a[text()='Share Skill']";
        private readonly string toastXPath = "//div[contains(@class,'ns-box ns-growl') and contains(@class,'ns-show')]";

        private IWebElement NotificationIcon => _driver.FindElement(By.XPath("//i[@class='bell icon']"));
      
        // Main Tabs
        private IWebElement DashboardTab => _driver.FindElement(By.XPath("//a[text()='Dashboard']"));
        private IWebElement ProfileTab => _driver.FindElement(By.XPath("//a[text()='Profile']"));
        private IWebElement ManageListingsTab => _driver.FindElement(By.XPath("//a[text()='Manage Listings']"));
        private IWebElement ManageRequestsTab => _driver.FindElement(By.XPath("//div[contains(@class,'ui dropdown') and contains(text(),'Manage Requests')]"));

        // Click Actions
        public void ClickSearchSkill()
        {
           
            Wait.WaitToBeClickable(_driver, "XPath",searchSkillIconXPath, 10);
            _driver.FindElement(By.XPath(searchSkillIconXPath)).Click();

        }

      
        public void ClickShareSkill()
        {
           
            for (int i = 0; i < 3; i++) // retry up to 3 times
            {
                try
                {
                    // Wait until Share Skill button is clickable
                    Wait.WaitToBeClickable(_driver, "XPath", shareSkillXPath, 15);

                    // Wait for any visible toast notifications to disappear
                    var toastElements = _driver.FindElements(By.XPath(toastXPath));
                    if (toastElements.Count > 0)
                    {
                        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                        foreach (var toast in toastElements)
                        {
                            wait.Until(drv => !toast.Displayed);
                        }
                    }

                    // Click the Share Skill button
                    _driver.FindElement(By.XPath(shareSkillXPath)).Click();
                    return; // success → exit method
                }
                catch (StaleElementReferenceException)
                {
                    if (i == 2) throw; // rethrow if this was the last attempt
                    Thread.Sleep(300); // short wait before retry
                }
                catch (ElementClickInterceptedException)
                {
                    // If click is intercepted, wait a short time and retry
                    Thread.Sleep(500);
                }
            }
        }


        public void OpenNotifications() => NotificationIcon.Click();

        public void GoToDashboard() => DashboardTab.Click();
        public void GoToProfile() => ProfileTab.Click();
        public void GoToManageListings() => ManageListingsTab.Click();
      
        public void GoToManageRequests(string requestType)
        {
            try
            {
                //// 1. Click the "Manage Requests" dropdown to reveal options
             
                ManageRequestsTab.Click();

                // 2. Determine which link to click based on requestType
                string linkXPath = "";
                if (requestType.Equals("Received", StringComparison.OrdinalIgnoreCase))
                {
                    linkXPath = "//a[@href='/Home/ReceivedRequest']";
                }
                else if (requestType.Equals("Sent", StringComparison.OrdinalIgnoreCase))
                {
                    linkXPath = "//a[@href='/Home/SentRequest']";
                }
                else
                {
                    throw new ArgumentException($"Invalid requestType: {requestType}. Use 'Received' or 'Sent'.");
                }
                // Wait for the link to be clickable
                Wait.WaitToBeClickable(_driver, "XPath", linkXPath, 10);

                // Find the link again and click
                var requestLink = _driver.FindElement(By.XPath(linkXPath));
                requestLink.Click();

                Console.WriteLine($"Navigated to {requestType} Requests page.");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Request link not found for {requestType} Requests.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error navigating to {requestType} Requests: {ex.Message}");
                throw;
            }
        }

        public void SignOut()
        {
                    
            Thread.Sleep(2000); 
          
            _driver.FindElement(By.XPath(signOutButtonXPath)).Click();
        }

        public bool IsSignOutVisible()
        {
            try
            {
                var signOutElement = _driver.FindElement(By.XPath(signOutButtonXPath));
                return signOutElement.Displayed && signOutElement.Enabled;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }


    }

}
