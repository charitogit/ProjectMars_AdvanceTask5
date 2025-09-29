using AventStack.ExtentReports.Model;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using ProjectMars_AdvanceTask_NUnit.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Pages.SearchSkill
{
    public class SearchSkillPage
    {
        private readonly IWebDriver _driver;
        private readonly TestStateInfo _state;  

        //XPath locators
        private readonly string topSearchBoxInputXPath = "(//input[@placeholder='Search skills'])[1]"; //first search box 
        private readonly string topSearchButtonXPath ="//*[@id='service-search-section']//div[contains(@class,'search-box')]//i[contains(@class,'search')]";

        private readonly string filterSearchBoxXPath = "(//input[@placeholder='Search skills'])[2]";  //second search box
        private readonly string filterSearchButtonXPath = "(//i[contains(@class,'search link icon')])[2]";  //second find button icon


        private readonly string categoryXPath = "//div[@role='list']/a[contains(@class,'category')]";
        private readonly string subCategoryXPath = "//div[@role='list']/a[contains(@class,'subcategory')]";
        private readonly string searchResultsXPath = "//div[contains(@class,'result-card')]";

        private readonly string allCategoryOptionXPath = "//b[normalize-space()='All Categories']"; //all category option in the list
        private readonly string allCategoryOptionCountXPath = "//a[@role='listitem' and contains(@class, 'active')][.//b[normalize-space()='All Categories']]//span"; //count of all category option in the list

     
        private readonly string searchResultTitleXPath = "//div[contains(@class,'ui stackable three cards')]//p[contains(@class,'row-padded')]"; //search result title
        private readonly string searchResultCardsXPath = "//div[contains(@class,'ui stackable three cards')]/div[contains(@class,'card')]";


        //Element Fields 
        private IWebElement TopSearchBoxInput, TopSearchButton;
        private IWebElement FilterSearchBoxInput, FilterSearchButton;
        private IWebElement AllCategoryOption, AllCategoryOptionCount;
        private IWebElement SelectedSubCategoryOptionCount;
        private IWebElement FilterLocationTypeButton; 
        private IWebElement SearchResultTitle, SearchResultCardsContainer;
        private IWebElement ResultMessage;


        public SearchSkillPage(IWebDriver driver,TestStateInfo state)
        {
            _driver = driver;
            _state = state;
        }

        //Render Static Elements 
        public void RenderStaticElements()
        {
            TopSearchBoxInput = FindVisibleElement(topSearchBoxInputXPath);
            TopSearchButton = FindClickableElement(topSearchButtonXPath);
            FilterSearchBoxInput = FindVisibleElement(filterSearchBoxXPath);
            FilterSearchButton = FindClickableElement(filterSearchButtonXPath);
           
            AllCategoryOption = FindExistingElement(allCategoryOptionXPath);
        }

        //Private reusable helper methods
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

        private IReadOnlyCollection<IWebElement> GetCategoryList()
        {
            Wait.WaitToBeVisible(_driver, "XPath", categoryXPath, 5);
            return _driver.FindElements(By.XPath(categoryXPath));
        }

        private IReadOnlyCollection<IWebElement> GetSubCategoryList()
        {
            Wait.WaitToBeVisible(_driver, "XPath", subCategoryXPath, 5);
            return _driver.FindElements(By.XPath(subCategoryXPath));
        }

        private IReadOnlyCollection<IWebElement> GetSearchResults()
        {
            Wait.WaitToBeVisible(_driver, "XPath", searchResultsXPath, 5);
            return _driver.FindElements(By.XPath(searchResultsXPath));
        }

        // --- Search Actions ---
       
        public void DoSearchByAllCategory()
        {
            RenderStaticElements();
            EnterKeyWordTopSearchBox();
            ClickHelper.ScrollIntoViewAndClick(_driver, TopSearchButton);
        }
        public void DoSearchBySubCategory()
        {
            RenderStaticElements();
            SelectCategory();
            SelectSubCategory();
            EnterKeyWordFilterSearchBox();
            ClickHelper.ScrollIntoViewAndClick(_driver, FilterSearchButton);
        }

       public void ClickCardByTitle(string title)
        {
            // Wait for the search results to be present
            Wait.WaitToBeVisible(_driver, "XPath", searchResultTitleXPath, 5);
            // Find all cards and click the one with the matching title
            var cards = _driver.FindElements(By.XPath(searchResultTitleXPath));
            foreach (var card in cards)
            {
                if (card.Text.Trim().Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    card.Click();
                    return; // exit after clicking the first matching card
                }
            }
            throw new NoSuchElementException($"No card found with title: {title}");
        }

        public void ClickFilterLocationTypeButton()
        {            
            string filterLocationTypeButtonXPath = $"//div[@class='ui buttons']//button[normalize-space()='{_state.CurrentSearchSkill.LocationType}']";

            FilterLocationTypeButton = FindClickableElement(filterLocationTypeButtonXPath);
            ClickHelper.ScrollIntoViewAndClick(_driver, FilterLocationTypeButton);
        }   


        private void EnterKeyWordTopSearchBox()
        {
            TopSearchBoxInput.Clear();
            TopSearchBoxInput.SendKeys(_state.CurrentSearchSkill.Keyword);
           
        }
        private void EnterKeyWordFilterSearchBox()
        {
            FilterSearchBoxInput.Clear();
            FilterSearchBoxInput.SendKeys(_state.CurrentSearchSkill.Keyword);
       
        }

       
        private void SelectCategory()
        {
            string categoryName = _state.CurrentSearchSkill.Category;
            var categories = GetCategoryList();
            foreach (var category in categories)
            {
                // Clean the text to remove newlines and extra spaces
                string actualCategoryName = category.Text.Split('\r', '\n')[0].Trim();

                // Compare using the cleaned text
                if (actualCategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase))
                {
                    category.Click();
                    break;
                }
            }
        }

      private void SelectSubCategory()
{
    string subCategoryName = _state.CurrentSearchSkill.SubCategory;
    var subCategories = GetSubCategoryList();

    foreach (var subCategory in subCategories)
    {
        // Take everything before the first newline (\r or \n) and trim
        string actualSubCategoryName = subCategory.Text.Split('\r', '\n')[0].Trim();

        if (actualSubCategoryName.Equals(subCategoryName, StringComparison.OrdinalIgnoreCase))
        {
            subCategory.Click();
            break;
        }
    }
}


        public bool AreSearchResultsPresent()
        {
            var results = GetSearchResults();
            return results.Count > 0;
        }

        //For Assertions and validations
        public int GetAllCategoryOptionCount()
        {
            AllCategoryOptionCount =FindVisibleElement(allCategoryOptionCountXPath);
           
            string countText = AllCategoryOptionCount.Text.Trim();
            
            return Convert.ToInt32(countText);
        }

        public int GetSubCategoryOptionCount()
        {            
            string subCategoryOptionCountXPath = $"//a[contains(@class,'subcategory')][contains(.,'{_state.CurrentSearchSkill.SubCategory}')]/span";
            
            SelectedSubCategoryOptionCount = FindVisibleElement(subCategoryOptionCountXPath);
            
            string countText = SelectedSubCategoryOptionCount.Text.Trim();

            return Convert.ToInt32(countText);
        }


        public bool AreAllCardsContainingKeyword(string keyword)
        {
            keyword = keyword.ToLower(); // make case-insensitive
            var cards = _driver.FindElements(By.XPath("//div[contains(@class,'ui card')]"));

            foreach (var card in cards)
            {
                // a.Check title in search result
                var titleElement = card.FindElement(By.XPath(".//a[contains(@class,'service-info')]/p"));
                string titleText = titleElement.Text.Trim().ToLower();

                if (titleText.Contains(keyword))
                    continue; // keyword found in title, next card

                // b. Open details page in new tab
                var cardLink = card.FindElement(By.XPath(".//a[contains(@class,'service-info')]")).GetAttribute("href");
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.open(arguments[0])", cardLink);

                // Switch to new tab
                _driver.SwitchTo().Window(_driver.WindowHandles.Last());

                // c. Wait for full content to load
                // Wait for any of the detail fields to appear
                Wait.WaitToBeVisible(_driver, "XPath", "//div[div[@class='header' and text()='Description']]/div[@class='description']", 5);

                // Collect description, category, subcategory, and tags
                var detailElements = _driver.FindElements(By.XPath(
                    "//div[div[@class='header' and text()='Description']]/div[@class='description'] | " +
                    "//div[div[@class='header' and text()='Category']]/div[@class='description'] | " +
                    "//div[div[@class='header' and text()='Subcategory']]/div[@class='description'] | " +
                    "//div[@class='extra content']//em"
                ));

                bool keywordFound = detailElements.Any(f => f.Text.ToLower().Contains(keyword));

                // Close the detail tab and return to results
                _driver.Close();
                _driver.SwitchTo().Window(_driver.WindowHandles.First());

                if (!keywordFound)
                    return false; // fail if keyword not found anywhere
            }

            return true; // keyword found in all cards
        }

        public string GetResultMessage()
        {
            // Wait for the result message to be visible
            string resultMessageXPath = "//h3[normalize-space()='No results found, please select a new category!']";  
           
            ResultMessage = FindVisibleElement(resultMessageXPath);
           
            return ResultMessage.Text.Trim();
        }










    }
}
