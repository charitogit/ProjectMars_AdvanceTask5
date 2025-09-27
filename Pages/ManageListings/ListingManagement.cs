using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using ProjectMars_AdvanceTask_NUnit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NUnit.Framework.Constraints.NUnitEqualityComparer;

namespace ProjectMars_AdvanceTask_NUnit.Pages.ManageListings
{
    public  class ListingManagement
    {
        private readonly IWebDriver _driver;
        private readonly TestStateInfo _state;

        public ListingManagement(IWebDriver driver,TestStateInfo state)
        {
            _driver= driver;
            _state = state;

         }

        public bool IsRecordPresent()
        {
            string uiServiceType = _state.CurrentShareSkill.ServiceType == "Hourly basis service" ? "Hourly" : "One-off";
            string firstRowXPath = $"//table/tbody/tr[1][td[2][normalize-space()='{_state.CurrentShareSkill.Category}'] and td[3][normalize-space()='{_state.CurrentShareSkill.Title}'] and td[4][normalize-space()='{_state.CurrentShareSkill.Description}'] and td[5][normalize-space()='{uiServiceType}']]";

            Wait.WaitToBeVisible(_driver, "XPath", firstRowXPath, 10);

            return ElementExists(firstRowXPath);

        }

        private bool ElementExists(string xpath)
        {
            try
            {
                return _driver.FindElements(By.XPath(xpath)).Count > 0;
            }
            catch
            {
                return false;
            }
        }

        public void DeleteSpecificShareSkillListings(List<ShareSkillModel> shareSkills)
        {
            try
            {
                if (shareSkills == null || shareSkills.Count == 0)
                    return;

                
                foreach (var skill in shareSkills)
                {
                    // Locate delete button for the row that matches both Title and Description
                    string deleteButtonXPath = $"//table/tbody/tr[td[2][normalize-space()='{skill.Category}'] and td[3][normalize-space()='{skill.Title}'] and td[4][normalize-space()='{skill.Description}']]//i[contains(@class,'remove icon')]";

                    var deleteButtons = _driver.FindElements(By.XPath(deleteButtonXPath));

                    if (deleteButtons.Count > 0)
                    {
                        Wait.WaitToBeClickable(_driver, "XPath", deleteButtonXPath, 5);
                        deleteButtons[0].Click();

                        // Click "Yes" in confirmation modal
                        string yesButtonXPath = "//div[contains(@class, 'ui tiny modal')]//button[contains(text(), 'Yes')]";
                        Wait.WaitToBeClickable(_driver, "XPath", yesButtonXPath, 5);
                        _driver.FindElement(By.XPath(yesButtonXPath)).Click();

                        Console.WriteLine($"[Cleanup] Deleted ShareSkill - Title: {skill.Title}, Description: {skill.Description}");
                    }
                    else
                    {
                        Console.WriteLine($"[Cleanup] No ShareSkill found - Title: {skill.Title}, Description: {skill.Description}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cleanup] ERROR deleting ShareSkill listings: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }


        public void DeleteAllSkillListing()
        {
            try
            {
                string rowsXPath = "//table[contains(@class, 'ui striped table')]/tbody/tr";
                var rows = _driver.FindElements(By.XPath(rowsXPath));
                int maxFailuresAllowed = 3;
                int failureCount = 0;

                while (rows.Count > 0)
                {
                    // Click the delete icon in the first row
                    var deleteButton = rows[0].FindElement(By.XPath(".//td//i[contains(@class, 'remove icon')]"));
                    Wait.WaitToBeClickable(_driver, "XPath", ".//td//i[contains(@class, 'remove icon')]", 5);
                    deleteButton.Click();

                    // Wait for and click the "Yes" button in the modal
                    string yesButtonXPath = "//div[contains(@class, 'ui tiny modal')]//button[contains(text(), 'Yes')]";
                    Wait.WaitToBeClickable(_driver, "XPath", yesButtonXPath, 5);
                    var yesButton = _driver.FindElement(By.XPath(yesButtonXPath));
                    yesButton.Click();

                    // Check for failure toast message
                    string toastXPath = "//div[contains(@class,'ns-box-inner')]";
                    Wait.WaitToBeVisible(_driver, "XPath", toastXPath, 5);
                    var toastMessages = _driver.FindElements(By.XPath(toastXPath));
                    var failureToast = toastMessages.FirstOrDefault(msg => msg.Text.Contains("Unable to delete listing"));

                    if (failureToast != null)
                    {
                        failureCount++;
                        // Close the toast
                        string closeIconXPath = "//div[contains(@class,'ns-box')]//i[contains(@class,'close')]";
                        Wait.WaitToBeVisible(_driver, "XPath", closeIconXPath, 5);
                        var closeIcon = _driver.FindElement(By.XPath(closeIconXPath));
                        closeIcon.Click();

                        if (failureCount >= maxFailuresAllowed)
                        {
                            Console.WriteLine($"Max failed delete attempts reached ({failureCount}). Aborting to prevent infinite loop.");
                            break;
                        }
                    }

                    Console.WriteLine($"Deleting share skill record at {DateTime.Now}");
                                   
                    // Re-fetch updated list of rows
                    rows = _driver.FindElements(By.XPath(rowsXPath));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to delete skill: {ex.Message}");
                Console.WriteLine($"[STACK TRACE] {ex.StackTrace}");
                throw; // Re-throw to test to fail properly
            }
        }


    }
}
