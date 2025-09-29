using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using ProjectMars_AdvanceTask_NUnit.Utilities;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Pages.Components
{
    public class LanguagePage
    {
        private readonly IWebDriver _driver;
        private readonly TestStateInfo _state;

        // ========== XPath Locators ==========
        private const string ActiveLanguageTabXPath = "//div[@data-tab='first' and contains(@class, 'active')]";
        private string addNewButtonXPath => $"{ActiveLanguageTabXPath}//div[normalize-space()='Add New']";

        private const string LanguageInputXPath = "//input[@placeholder='Add Language']";
       
        private const string LevelDropdownXPath = "//select[@name='level']";
       
        private const string AddButtonXPath = "//input[@value='Add']";
        private const string UpdateButtonXPath = "//*[@value='Update']";
        private const string CancelButtonXPath = "//*[@value='Cancel']";
        private const string MessageBoxXPath = "//div[@class='ns-box-inner']";
        private const string MessageCloseIconXPath = "//*[@class='ns-close']";

        // ========== Element Fields ==========
        private IWebElement addNewRecordButton;
        private IWebElement editRecordButton;
        private IWebElement languageTextBox;
        private IWebElement levelDropDown;
        private IWebElement addButton;
        private IWebElement updateButton;
        private IWebElement cancelButton;

        // ========== Constructor ==========
        public LanguagePage(IWebDriver driver,TestStateInfo state)
        {
            _driver = driver;
            _state = state; 
        }

        // ========== Main Public Methods ==========
        public void AddLanguage()
        {
            ClickAddNewLanguageButton();
            RenderFormElements();
            FillLanguageForm();
        }

        public void EditLanguage(Languages originalLanguage)
        {
            ClickEditButtonForRecord(originalLanguage);
            RenderFormElements();
                      
            FillLanguageForm();

        }

     
        public void DeleteAllLanguage()
        {
            try
            {
                string rowsXPath = $"{ActiveLanguageTabXPath}//table[@class='ui fixed table']/tbody/tr";
                var rows = _driver.FindElements(By.XPath(rowsXPath));

                while (rows.Count > 0)
                {
                    string deleteButtonXPath = $"{rowsXPath}[1]//td//i[contains(@class, 'remove icon')]";
                    Wait.WaitToBeClickable(_driver, "XPath", deleteButtonXPath, 5);

                    var deleteButton = rows[0].FindElement(By.XPath(".//td//i[contains(@class, 'remove icon')]"));
                    deleteButton.Click();

                    Wait.WaitToBeVisible(_driver, "XPath", "//div[contains(@class,'ns-show')]", 5);

                    rows = _driver.FindElements(By.XPath(rowsXPath));
                    Console.WriteLine($"Deleting language record at {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to delete language: {ex.Message}");
                Console.WriteLine($"[STACK TRACE] {ex.StackTrace}");
                throw;  // Re-throw to test to fail properly
            }
        }


        public void DeleteLanguageIfExists(Languages data)
        {
            try
            {
              
                //locate exact specific record as per data parameter
                string deleteButtonXPath = $"{ActiveLanguageTabXPath}//table/tbody/tr[td[1][normalize-space()='{data.LanguageName}'] and td[2][normalize-space()='{data.Level}']]//i[@class='remove icon']";

                var deleteButtons = _driver.FindElements(By.XPath(deleteButtonXPath));
                if (deleteButtons.Count > 0)
                {
                    Wait.WaitToBeClickable(_driver, "XPath", deleteButtonXPath, 5);
                    deleteButtons[0].Click();

                    Console.WriteLine($"[Cleanup] Deleted Language data - Language: {data.LanguageName}, Level: {data.Level}");
                }
                else
                {
                    Console.WriteLine($"[Cleanup] No record found Language data -  Language: {data.LanguageName}, Level: {data.Level}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Cleanup] ERROR deleting Language (safe): " + ex.Message);
            }
        }




        // ========== Public Actions ==========

        public void ClickAddNewLanguageButton()
        {
            Wait.WaitToBeVisible(_driver, "XPath", ActiveLanguageTabXPath, 10);
            addNewRecordButton = FindClickableElement(addNewButtonXPath, 10);
            addNewRecordButton.Click();
        }

        public void ClickEditButtonForRecord(Languages originalLanguage)
        {
            Wait.WaitToBeVisible(_driver, "XPath", ActiveLanguageTabXPath, 10);

            string editButtonXPath = $"//table/tbody/tr[" +
                $"td[1][normalize-space()='{originalLanguage.LanguageName}'] and " +
                $"td[2][normalize-space()='{originalLanguage.Level}']" +
                $"]//i[@class='outline write icon']";

            editRecordButton = FindClickableElement(editButtonXPath, 10);
            editRecordButton.Click();
        }
        public void FillLanguageForm()
        {
            languageTextBox.Click();
            languageTextBox.SendKeys(Keys.Control + "a");
            languageTextBox.SendKeys(Keys.Delete);
            languageTextBox.SendKeys(_state.CurrentLanguage.LanguageName);

            SelectDropDownByText(levelDropDown, _state.CurrentLanguage.Level);

            SubmitForm(); 

        }


        public bool IsRecordPresent()
        {
            string tableXPath = $"{ActiveLanguageTabXPath}//table/tbody";
            string recordXPath = $"//table/tbody/tr[td[1][normalize-space()='{_state.CurrentLanguage.LanguageName}'] and td[2][normalize-space()='{_state.CurrentLanguage.Level}']]";


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
            languageTextBox = FindVisibleElement(LanguageInputXPath);
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
