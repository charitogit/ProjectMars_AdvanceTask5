using AngleSharp.Dom;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ProjectMars_AdvanceTask_NUnit.Pages.Profile
{

    public class ProfileDetailComponent
    {
        private readonly IWebDriver _driver;


        private const string MessageBoxXPath = "//div[@class='ns-box-inner']";
        private const string ProfileFieldValueXPath = "//*[@id='account-profile-section']//div/span[contains(normalize-space(text()), '";

        private IWebElement LocationField => _driver.FindElement(By.XPath("//input[@name='location']"));

        // Edit icons (to click for opening the dropdowns)
        private IWebElement AvailabilityEditIcon => _driver.FindElement(By.XPath("//div[@class='item'][.//strong[normalize-space(text())='Availability']]//i[contains(@class, 'write icon')]"));
        private IWebElement HoursEditIcon => _driver.FindElement(By.XPath("//div[@class='item'][.//strong[normalize-space(text())='Hours']]//i[contains(@class, 'write icon')]"));
        private IWebElement EarnTargetEditIcon => _driver.FindElement(By.XPath("//div[@class='item'][.//strong[normalize-space(text())='Earn Target']]//i[contains(@class, 'write icon')]"));

        // Dropdowns (actual <select> elements, used for selection)
        private IWebElement AvailabilityDropdown => _driver.FindElement(By.Name("availabiltyType"));
        private IWebElement HoursDropdown => _driver.FindElement(By.Name("availabiltyHour"));
        private IWebElement EarnTargetDropdown => _driver.FindElement(By.Name("availabiltyTarget"));


        public ProfileDetailComponent(IWebDriver driver)
        {
            _driver = driver;
        }


        public void SelectAvailability(string availability)
        {
            AvailabilityEditIcon.Click();

            var select = new SelectElement(AvailabilityDropdown);

            if (select.SelectedOption.Text.Trim() != availability.Trim())
            {
                select.SelectByText(availability);
               
            }
            ClickHelper.FireJsEvent(_driver,AvailabilityDropdown, "change");
        }


        public void SelectHours(string hours)
        {
            HoursEditIcon.Click();

            var select = new SelectElement(HoursDropdown);
            if (select.SelectedOption.Text.Trim() != hours.Trim())
            {
                select.SelectByText(hours);
            }
            ClickHelper.FireJsEvent(_driver,HoursDropdown, "change");
        }

        public void SelectEarnTarget(string targetValue)
        {
            EarnTargetEditIcon.Click();

            var select = new SelectElement(EarnTargetDropdown);
            if (select.SelectedOption.Text.Trim() != targetValue.Trim())
            {
                select.SelectByText(targetValue);
            }
            ClickHelper.FireJsEvent(_driver,EarnTargetDropdown, "change");
        }


        public string GetActualMessage()
        {

            Wait.WaitToBeVisible(_driver, "XPath", MessageBoxXPath, 10);
            return _driver.FindElement(By.XPath(MessageBoxXPath)).Text.Trim();
        }

        public string GetActualAvailability(string availability)
        {
           var element = _driver.FindElement(By.XPath($"{ProfileFieldValueXPath}{availability}')]")); //interpolate base field xpath and its dynamic text value 
              
            return element.Text.Trim();
        }
        public string GetActualHours(string hours)
        {
            var element = _driver.FindElement(By.XPath($"{ProfileFieldValueXPath}{hours}')]"));  //interpolate base field xpath and its dynamic text value 
            return element.Text.Trim();
        }
        public string GetActualEarnTarget(string eartTarget)
        {
            var element = _driver.FindElement(By.XPath($"{ProfileFieldValueXPath}{eartTarget}')]")); //interpolate base field xpath and its dynamic text value 
            return element.Text.Trim();

        }
        private string GetSelectedTextFromDropdown(IWebElement dropdown)
        {
            return new SelectElement(dropdown).SelectedOption.Text.Trim();
        }


      
    }

}