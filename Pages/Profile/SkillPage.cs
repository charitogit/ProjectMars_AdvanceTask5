using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using ProjectMars_AdvanceTask_NUnit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Pages.Components
{
    public class SkillPage
    {
        private readonly IWebDriver _driver;
        private readonly TestStateInfo _state;

        // ========== XPath Locators ==========
        private const string ActiveSkillTabXPath = "//div[@data-tab='second' and contains(@class, 'active')]";
        private string addNewButtonXPath => $"{ActiveSkillTabXPath}//div[normalize-space()='Add New']";
        private const string SkilllInputXPath = "//input[@placeholder='Add Skill']";
        private const string LevelDropdownXPath = "//select[@name='level']";
        private const string AddButtonXPath = "//input[@value='Add']";
        private const string UpdateButtonXPath = "//*[@value='Update']";
        private const string CancelButtonXPath = "//*[@value='Cancel']";
        private const string MessageBoxXPath = "//div[@class='ns-box-inner']";
        private const string MessageCloseIconXPath = "//*[@class='ns-close']";

        // ========== Element Fields ==========
        private IWebElement addNewRecordButton;
        private IWebElement editRecordButton;
        private IWebElement skillTextBox;
        private IWebElement levelDropDown;
        private IWebElement addButton;
        private IWebElement updateButton;
        private IWebElement cancelButton;

        // ========== Constructor ==========
        public SkillPage(IWebDriver driver,TestStateInfo state)
        {
            _driver = driver;
            _state = state;
        }

        // ========== Main Public Methods ==========
        public void AddSkill()
        {
            ClickAddNewSkillButton();
            RenderFormElements();
            FillSkillForm();
        }

        public void EditSkill(Skill originalSkill)
        {
            ClickEditButtonForRecord(originalSkill);
            RenderFormElements();

            FillSkillForm(); // Uses _state.CurrentSkill

        }
        public void DeleteAllSkill()
        {
            try
            {
                string rowsXPath = $"{ActiveSkillTabXPath}//table[@class='ui fixed table']/tbody/tr";
                var rows = _driver.FindElements(By.XPath(rowsXPath));

                while (rows.Count > 0)
                {
                    string deleteButtonXPath = $"{rowsXPath}[1]//td//i[contains(@class, 'remove icon')]";
                    Wait.WaitToBeClickable(_driver, "XPath", deleteButtonXPath, 5);

                    var deleteButton = rows[0].FindElement(By.XPath(".//td//i[contains(@class, 'remove icon')]"));
                    deleteButton.Click();

                    Wait.WaitToBeVisible(_driver, "XPath", "//div[contains(@class,'ns-show')]", 5);

                    rows = _driver.FindElements(By.XPath(rowsXPath));
                    Console.WriteLine($"Deleting skill record at {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to delete skill: {ex.Message}");
                Console.WriteLine($"[STACK TRACE] {ex.StackTrace}");
                throw; // Re-throw to test to fail properly
            }
        }


        public void DeleteSkillIfExists(Skill data)
        {
            try
            {

                string activeTabContentXPath = "//div[@data-tab='second' and contains(@class, 'active')]";
                //locate exact specific record as per data parameter
                string deleteButtonXPath = $"{activeTabContentXPath}//table/tbody/tr[td[1][normalize-space()='{data.SkillName}'] and td[2][normalize-space()='{data.Level}']]//i[@class='remove icon']";

                var deleteButtons = _driver.FindElements(By.XPath(deleteButtonXPath));
                if (deleteButtons.Count > 0)
                {
                    Wait.WaitToBeClickable(_driver, "XPath", deleteButtonXPath, 5);
                    deleteButtons[0].Click();

                    Console.WriteLine($"[Hook:After Cleanup] Deleted Skill data - Skill: {data.SkillName}, Level: {data.Level}");
                }
                else
                {
                    Console.WriteLine($"[Hook:After Cleanup] No record found Skill data -  Skill: {data.SkillName}, Level: {data.Level}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Hook:After Cleanup] ERROR deleting Skill (safe): " + ex.Message);
            }
        }




        // ========== Public Actions ==========

        public void ClickAddNewSkillButton()
        {
            Wait.WaitToBeVisible(_driver, "XPath", ActiveSkillTabXPath, 10);
            addNewRecordButton = FindClickableElement(addNewButtonXPath, 10);
            addNewRecordButton.Click();
        }

        public void ClickEditButtonForRecord(Skill originalSkill)
        {
            Wait.WaitToBeVisible(_driver, "XPath", ActiveSkillTabXPath, 10);

            string editButtonXPath = $"//table/tbody/tr[" +
                $"td[1][normalize-space()='{originalSkill.SkillName}'] and " +
                $"td[2][normalize-space()='{originalSkill.Level}']" +
                $"]//i[@class='outline write icon']";

            editRecordButton = FindClickableElement(editButtonXPath, 10);
            editRecordButton.Click();
        }
        public void FillSkillForm()
        {
            skillTextBox.Click();
            skillTextBox.SendKeys(Keys.Control + "a");
            skillTextBox.SendKeys(Keys.Delete); 
            skillTextBox.SendKeys(_state.CurrentSkill.SkillName);

            SelectDropDownByText(levelDropDown, _state.CurrentSkill.Level);

            SubmitForm(); 

        }

        public bool IsRecordPresent()
        {
            string tableXPath = $"{ActiveSkillTabXPath}//table/tbody";
            string recordXPath = $"//table/tbody/tr[td[1][normalize-space()='{_state.CurrentSkill.SkillName}'] and td[2][normalize-space()='{_state.CurrentSkill.Level}']]";


            try
            {
                // Wait for the table itself to appear (not necessarily the specific record)
                Wait.WaitToBeVisible(_driver, "XPath", tableXPath, 5);

                // After confirming table is visible, search for the specific record
                Wait.WaitToBeVisible(_driver, "XPath", recordXPath, 10);
                return _driver.FindElements(By.XPath(recordXPath)).Count > 0;
            }

            catch (WebDriverTimeoutException)
            {
                // Table never appeared → Treat as no record
                return false;
            }

        }

        // ========== Helper Methods ==========
        private void RenderFormElements()
        {
            skillTextBox = FindVisibleElement(SkilllInputXPath);
            levelDropDown = FindVisibleElement(LevelDropdownXPath);
         

            if (ElementExists(AddButtonXPath)) addButton = FindClickableElement(AddButtonXPath, 10);
            if (ElementExists(UpdateButtonXPath)) updateButton = FindClickableElement(UpdateButtonXPath, 10);
            if (ElementExists(CancelButtonXPath)) cancelButton = FindClickableElement(CancelButtonXPath, 10);
        }

        private IWebElement FindVisibleElement(string xpath, int timeout = 10)
        {
            Wait.WaitToBeVisible(_driver, "XPath", xpath, timeout);
            return _driver.FindElement(By.XPath(xpath));
        }

        private IWebElement FindClickableElement(string xpath, int timeout = 10)
        {
            Wait.WaitToBeClickable(_driver, "XPath", xpath, timeout);
            return _driver.FindElement(By.XPath(xpath));
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

        private void SelectDropDownByText(IWebElement dropdown, string text)
        {
            if (string.IsNullOrEmpty(text))
                return; 

            var selectElement = new SelectElement(dropdown);
            selectElement.SelectByText(text);
        }

        private void SubmitForm()
        {
            if (ElementExists(AddButtonXPath))
                FindClickableElement(AddButtonXPath).Click();
            else if (ElementExists(UpdateButtonXPath))
                FindClickableElement(UpdateButtonXPath).Click();
        }


        // ========== Message & Notification Handling ==========
        public string GetActualMessage()
        {
            Wait.WaitToBeVisible(_driver, "XPath", MessageBoxXPath, 5);
            return _driver.FindElement(By.XPath(MessageBoxXPath)).Text.Trim();
        }


        public void CloseMessage()
        {
            if (ElementExists(MessageCloseIconXPath))
            {
                _driver.FindElement(By.XPath(MessageCloseIconXPath)).Click();
            }
        }

        public void ClickCancel() => cancelButton.Click();
    }
}
