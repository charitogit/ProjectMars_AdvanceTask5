using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using ProjectMars_AdvanceTask_NUnit.Utilities;
using SeleniumExtras.WaitHelpers;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using OpenQA.Selenium.Interactions;
using ProjectMars_AdvanceTask_NUnit.Tests;
using ProjectMars_AdvanceTask_NUnit.Helpers;

namespace ProjectMars_AdvanceTask_NUnit.Pages.Components
{
    public class ShareSkillPage
    {
        private readonly IWebDriver _driver;
        private readonly TestStateInfo _state;

        // ========== XPath Locators ==========
        private const string TitleInputXPath = "//input[@name='title']";
        private const string DescriptionInputXPath = "//textarea[@name='description']";
        private const string CategoryDropdownXPath = "//select[@name='categoryId']";
        private const string SubCategoryDropdownXPath = "//select[@name='subcategoryId']";
        private const string TagsInputXPath = "(//input[@placeholder='Add new tag'])[1]";  //first tag

        private const string ServiceTypeHourlyXPath = "//div[contains(@class,'ui radio checkbox')][label[normalize-space()='Hourly basis service']]//input[@name='serviceType']";
        private const string ServiceTypeOneOffXPath = "//div[contains(@class,'ui radio checkbox')][label[normalize-space()='One-off service']]//input[@name='serviceType']";

        private const string LocationTypeOnlineXPath = "//div[contains(@class,'ui radio checkbox')][label[normalize-space()='Online']]//input[@name='locationType']";
        private const string LocationTypeOnsiteXPath = "//div[contains(@class,'ui radio checkbox')][label[normalize-space()='On-site']]//input[@name='locationType']";

        private const string SkillTradeXPath = "//div[contains(@class,'ui radio checkbox')][label[normalize-space()='Skill-exchange']]//input[@name='skillTrades']";
        private const string CreditTradeXPath = "//div[contains(@class,'ui radio checkbox')][label[normalize-space()='Credit']]//input[@name='skillTrades']";
       
        private const string SkillExchangeInputXPath = "(//input[@placeholder='Add new tag'])[2]";  //second tag
        private const string CreditInputXPath = "//input[@name='charge']";

        private const string ActiveStatusXPath = "//div[contains(@class,'ui radio checkbox')][label[normalize-space()='Active']]//input[@name='isActive']";
        private const string InactiveStatusXPath = "//div[contains(@class,'ui radio checkbox')][label[normalize-space()='Hidden']]//input[@name='isActive']";

        private const string SaveButtonXPath = "//input[@value='Save']";
        private const string SuccessMessageXPath = "//div[contains(@class, 'ns-box-inner')]";
       
        private const string CancelButtonXPath = "//*[@value='Cancel']";
        private const string MessageBoxXPath = "//div[@class='ns-box-inner']";
        private const string InputMessageBoxXPath ="//div[(contains(@class, 'ui') and contains(@class, 'basic') and contains(@class, 'red') and contains(@class, 'label'))" +
            " or " + "(contains(@class, 'ui') and contains(@class, 'basic') and contains(@class, 'red') and contains(@class, 'prompt') and contains(@class, 'label') and contains(@class, 'transition') and contains(@class, 'visible'))]";


        private const string MessageCloseIconXPath = "//*[@class='ns-close']";



        // ========== Element Fields ==========
        private IWebElement titleInput, descriptionInput, categoryDropdown, subCategoryDropdown, tagsInput;
        private IWebElement serviceTypeHourly, serviceTypeOneOff;

        private IWebElement locationTypeOnline, locationTypeOnsite;
        private IWebElement skillTradeOption, creditTradeOption;
        private IWebElement skillExchangeInput, creditInput;
        private IWebElement activeStatus, hiddenStatus;
        private IWebElement saveButton, cancelButton;
        
        public object ScreenshotImageFormat { get; private set; }


        // ========== Constructor ==========
        public ShareSkillPage(IWebDriver driver, TestStateInfo state)
        {
            _driver = driver;
            _state = state;
        }

        // ========== Main Public Methods ==========

        public void FillAndSubmitShareSkill()
        {
            RenderFormElements();

            EnterTitle(_state.CurrentShareSkill.Title);
            EnterDescription(_state.CurrentShareSkill.Description);
            SelectCategory(_state.CurrentShareSkill.Category);
            
            // Only select subcategory if a category was selected
            if (!string.IsNullOrWhiteSpace(_state.CurrentShareSkill.Category) &&
                !string.IsNullOrWhiteSpace(_state.CurrentShareSkill.SubCategory))
            {
                SelectSubCategory(_state.CurrentShareSkill.SubCategory);
            }
            
            AddTags(_state.CurrentShareSkill.Tags);
                                
            SelectServiceType(_state.CurrentShareSkill.ServiceType);       // e.g., "Hourly" or "One-off"
            SelectLocationType(_state.CurrentShareSkill.LocationType);     // e.g., "Online" or "Onsite"

            ChooseSkillTrade(_state.CurrentShareSkill.SkillTradeType);          // e.g., "Skill-exchange" or "Credit"

            if (_state.CurrentShareSkill.SkillTradeType == "Skill-exchange")
            {
                AddSkillExchange(_state.CurrentShareSkill.SkillExchange);
            }

            else if (_state.CurrentShareSkill.SkillTradeType == "Credit")
            {
                EnterCredit(_state.CurrentShareSkill.Credit);
            }
          
            SetActiveStatus(_state.CurrentShareSkill.Status);            // e.g., true = Active, false = Hidden

            ClickSave();
        }

        public void FillAndEnterTitle(string title)
        {
            titleInput = FindVisibleElement(TitleInputXPath);
            EnterTitle(title); 
            
        }

        public void FillAndEnterDescription(string description)
        {
           
            descriptionInput = FindVisibleElement(DescriptionInputXPath);
            EnterDescription(description); 
        }

        
        public void FillAndEnterTags(List<string> tags)
        {
            tagsInput=FindVisibleElement(TagsInputXPath);

            foreach (var tag in tags)
            {
                EnterWithEnterKey(tagsInput, tag);
            }
        }

        public void FillAndEnterSkillExchangeTags(List<string> skillExchangetags)
        {
            skillExchangeInput = FindVisibleElement(SkillExchangeInputXPath);

            foreach (var tag in skillExchangetags)
            {
                EnterWithEnterKey(skillExchangeInput, tag);
            }
        }
        public void RemoveAllTags()
        {
            while (true)
            {
                var removeButtons = _driver.FindElements(By.XPath("//a[@class='ReactTags__remove']"));
                if (removeButtons.Count == 0)
                    break;

                removeButtons[0].Click();
            }
        }


      
        // ========== Helper Methods ==========
        private void RenderFormElements()
        {
            titleInput = FindVisibleElement(TitleInputXPath);
            descriptionInput = FindVisibleElement(DescriptionInputXPath);
           
            categoryDropdown = FindVisibleElement(CategoryDropdownXPath);

            tagsInput = FindVisibleElement(TagsInputXPath);

            serviceTypeHourly = FindExistingElement(ServiceTypeHourlyXPath);
            serviceTypeOneOff = FindExistingElement(ServiceTypeOneOffXPath);

            locationTypeOnline = FindExistingElement(LocationTypeOnlineXPath);
            locationTypeOnsite = FindExistingElement(LocationTypeOnsiteXPath);

            skillTradeOption = FindExistingElement(SkillTradeXPath);
            creditTradeOption = FindExistingElement(CreditTradeXPath);

            skillExchangeInput = FindVisibleElement(SkillExchangeInputXPath);
          
            activeStatus = FindExistingElement(ActiveStatusXPath);
            hiddenStatus = FindExistingElement(InactiveStatusXPath);

            saveButton = FindClickableElement(SaveButtonXPath);
            cancelButton = FindClickableElement(CancelButtonXPath);
        }

        // ========== Private Reusable Methods ==========
        private IWebElement FindVisibleElement(string xpath, int timeout = 10)
        {
            Wait.WaitToBeVisible(_driver, "XPath", xpath, timeout);
            return _driver.FindElement(By.XPath(xpath));
        }
        private IWebElement FindExistingElement(string xpath, int timeout = 10)
        {
            Wait.WaitToExist(_driver, "XPath", xpath, timeout);
            return _driver.FindElement(By.XPath(xpath));
        }

        private IWebElement FindClickableElement(string xpath, int timeout = 15)
        {
         
            Wait.WaitToBeClickable(_driver, "XPath", xpath, timeout);
            return _driver.FindElement(By.XPath(xpath));
        }

        private IWebElement GetRadioButton(string xpath, int timeout = 10)
        {
            Wait.WaitToBeClickable(_driver, "XPath", xpath, timeout);
            return _driver.FindElement(By.XPath(xpath));
        }



        private void EnterTitle(string title)
        {
            titleInput.Clear();
            titleInput.SendKeys(title);
        }

        private void EnterDescription(string description)
        {
            descriptionInput.Clear();
            descriptionInput.SendKeys(description);
        }

        private void SelectCategory(string category)
        {
            SelectDropDownByText(categoryDropdown, category);

            //Wait and Find only for Subcategory once Category is selected
            if (!string.IsNullOrWhiteSpace(category))
            {
                subCategoryDropdown = FindVisibleElement(SubCategoryDropdownXPath);
            }
         }

        private void SelectSubCategory(string subCategory)
        {
            SelectDropDownByText(subCategoryDropdown, subCategory);
        }

        // Accept multiple tags
        private void AddTags(string[] tags)
        {
            foreach (var tag in tags)
            {
                EnterWithEnterKey(tagsInput, tag);
            }
        }

        private void SelectServiceType(string serviceType)
        {
            if (serviceType == "Hourly basis service")
            {
                ClickHelper.ScrollIntoViewAndClick(_driver,serviceTypeHourly); 
            }
            else if (serviceType == "One-off service")
            {
                ClickHelper.ScrollIntoViewAndClick(_driver, serviceTypeOneOff);
            }
            else
            {
                throw new InvalidOperationException($"Unknown ServiceType: {serviceType}");
            }
        }
  private void SelectLocationType(string locationType)
        {
            if (locationType == "Online")
            {
                ClickHelper.ScrollIntoViewAndClick(_driver, locationTypeOnline);
            }
            else if (locationType == "On-site")
            {
                ClickHelper.ScrollIntoViewAndClick(_driver,locationTypeOnsite);
            }
            else
            {
                throw new InvalidOperationException($"Unknown LocationType: {locationType}");
            }
        }
         private void ChooseSkillTrade(string skillTradeType)
        {
            if (skillTradeType == "Skill-exchange")  
            {
                ClickHelper.ScrollIntoViewAndClick(_driver,skillTradeOption);

                skillExchangeInput = FindVisibleElement(SkillExchangeInputXPath); 
            }

            else if (skillTradeType == "Credit")
            {

                ClickHelper.ScrollIntoViewAndClick(_driver,creditTradeOption); 

                creditInput = FindVisibleElement(CreditInputXPath);
            }

            else
            {
                throw new InvalidOperationException($"Unknown SkillTradeType: {skillTradeType}");
            }
        }

     

    
        // This method adds tags to the Skill Exchange field, only if "Skill-exchange" is selected.
        // It also avoids adding duplicate tags that already exist in the UI.
        private void AddSkillExchange(string[] skillExchange)
        {
            // Check if the selected option is "Skill-exchange"
            if (_state.CurrentShareSkill.SkillTradeType == "Skill-exchange" && skillExchange != null)
            {
                foreach (var tag in skillExchange)
                {
                    // Only try to add if it's not empty and not already added
                    if (!string.IsNullOrWhiteSpace(tag) && !IsTagAlreadyPresent(tag))
                    {
                        // Type the tag into the input field
                        skillExchangeInput.SendKeys(tag.Trim());

                        // Press Enter to add the tag
                        skillExchangeInput.SendKeys(Keys.Enter);
                    }
                }
            }
        }

        // This method checks if a tag is already shown in the UI.
        // It returns true if the tag exists, false if not.
        private bool IsTagAlreadyPresent(string tagToCheck)
        {
            try
            {
                // Find all existing tags in the UI (adjust XPath if needed)
                var tagElements = _driver.FindElements(By.XPath("//span[contains(@class, 'label')]"));

                foreach (var tagElement in tagElements)
                {
                    // Compare the tag text with the tag we're trying to add (case-insensitive)
                    if (tagElement.Text.Trim().Equals(tagToCheck.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        return true; // Found a match — it's a duplicate
                    }
                }

                return false; // Not found — it's a new tag
            }
            catch (NoSuchElementException)
            {
                // No tags found at all — safe to add
                return false;
            }
        }


        private void EnterCredit(decimal? credit)
        {
            creditInput.Clear();
            creditInput.SendKeys(credit.ToString());
        }

        private void SetActiveStatus(string status)
        {
            if (status == "Active")
            {
                ClickHelper.ScrollIntoViewAndClick(_driver, activeStatus);  
               
            }
            else if (status == "Hidden")
            {
                ClickHelper.ScrollIntoViewAndClick(_driver, hiddenStatus);
            }
            else
            {
                throw new InvalidOperationException($"Unknown Status: {status}");
            }
        }
        private void SelectDropDownByText(IWebElement element, string text)
        {
            if (string.IsNullOrWhiteSpace(text)) 
                return; 

            var dropdown = new SelectElement(element);
            dropdown.SelectByText(text);
        }

        private void EnterWithEnterKey(IWebElement element, string text)
        {
            element.Clear();
            element.SendKeys(text);
            element.SendKeys(Keys.Enter);
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

       
        



        // ========== Assertion Message & Notification Handling ==========
        public string GetActualMessage()
        {
            Wait.WaitToBeVisible(_driver, "XPath", InputMessageBoxXPath, 5);
            return _driver.FindElement(By.XPath(InputMessageBoxXPath)).Text.Trim();
        }

        public void CloseMessage()
        {
            if (ElementExists(MessageCloseIconXPath))
            {
                _driver.FindElement(By.XPath(MessageCloseIconXPath)).Click();
            }
        }

        private void ClickSave() => saveButton.Click();
        public void ClickCancel() => cancelButton.Click();


        public string GetActualEnteredTitle()
        {
            
                Wait.WaitToBeVisible(_driver, "XPath", TitleInputXPath, 5);
                var element = _driver.FindElement(By.XPath(TitleInputXPath));
                return element?.GetAttribute("value")?.Trim() ?? string.Empty;
        }

        public string GetActualEnteredDescription()
        {
            Wait.WaitToBeVisible(_driver, "XPath", DescriptionInputXPath, 5);
            var element = _driver.FindElement(By.XPath(DescriptionInputXPath));
            return element?.GetAttribute("value")?.Trim() ?? string.Empty;
        }


        public bool TagIsInEditMode()
        {
            tagsInput = FindVisibleElement(TagsInputXPath);

            return tagsInput.Displayed && tagsInput.Enabled && !string.IsNullOrWhiteSpace(tagsInput.GetAttribute("value"));

        }

        public bool SkilllExchangeTagIsInEditMode()
        {
            skillExchangeInput = FindVisibleElement(SkillExchangeInputXPath);

            return skillExchangeInput.Displayed && skillExchangeInput.Enabled && !string.IsNullOrWhiteSpace(skillExchangeInput.GetAttribute("value"));

        }

        public bool AreAllTagsRemoved()
        {
            var tags = _driver.FindElements(By.XPath(TagsInputXPath));
            return tags.Count == 0;
        }

        public bool AreAllSkillExchangeTagsRemoved()
        {
            var tags = _driver.FindElements(By.XPath(SkillExchangeInputXPath));
            return tags.Count == 0;
        }


    }
}

