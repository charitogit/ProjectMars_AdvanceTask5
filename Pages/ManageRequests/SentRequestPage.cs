using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Pages.ManageRequests
{
    public class SentRequestPage
    {
        private readonly IWebDriver _driver; 

        public SentRequestPage(IWebDriver driver)
        {
            _driver = driver;
        }
        public void WithdrawRequest(string requestMessage)
        {
            try
            {
                // XPath to locate the table row containing the request message
                string rowXPath = $"//table[contains(@class,'sortable')]/tbody/tr[td[3][normalize-space()='{requestMessage}']]";

                // Wait for the row to be visible
                Wait.WaitToBeVisible(_driver, "XPath", rowXPath, 10);

                // Find the Withdraw button inside that row
                string withdrawButtonXPath = rowXPath + "//button[contains(text(),'Withdraw')]";
                var withdrawButton = _driver.FindElement(By.XPath(withdrawButtonXPath));

                // Click the Withdraw button
              
                ClickHelper.ScrollIntoViewAndClick(_driver, withdrawButton);

                Console.WriteLine($"Request with message '{requestMessage}' has been withdrawn.");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"No request found with message: {requestMessage}. Nothing to withdraw.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error withdrawing request: {ex.Message}");
                throw;
            }
        }

    }
}
