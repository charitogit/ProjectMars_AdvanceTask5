using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Pages.Profile;
using ProjectMars_AdvanceTask_NUnit.Pages.Shared;
using ProjectMars_AdvanceTask_NUnit.Utilities;


namespace ProjectMars_AdvanceTask_NUnit.Helpers
{
    public class NavigationHelper
    {
        private readonly IWebDriver _driver;
        //private readonly MainMenuTabsComponent _mainTabNavigation;
        private readonly MainNavigationComponent _mainNavigation;
        private readonly ProfilePage _profilePage;

        public NavigationHelper(IWebDriver driver)
        {
            _driver = driver;
            //_mainTabNavigation = new MainMenuTabsComponent(_driver);
            _mainNavigation = new MainNavigationComponent(_driver);
            _profilePage = new ProfilePage(_driver);
        }

        public void NavigateTo(string urlPath)
        {
            _driver.Navigate().GoToUrl(Hooks.TestBase.Settings.Environment.BaseUrl + urlPath);
        }

        public void GoToSignInPage()
        {
                    
            ClickHelper.ScrollIntoViewAndClick(_driver, _driver.FindElement(By.XPath("//a[contains(text(), 'Sign In')]")));
        }


        public void ClickCancelEdit()
        {
        
            string cancelButtonXpath = "//input[@value='Cancel' and @type='button']";
            Wait.WaitToBeClickable(_driver, "XPath", cancelButtonXpath, 10);
            var cancelButton = _driver.FindElement(By.XPath(cancelButtonXpath));
            cancelButton.Click();

        }

        public void NavigateToLanguageTab()
        {

            //_mainTabNavigation.RenderComponents();
            //_mainTabNavigation.ClickProfileTab();
            _mainNavigation.GoToProfile();
            _profilePage.Tabs.GoToLanguagesTab();

        }

        public void NavigateToSkillTab()
        {

            //_mainTabNavigation.RenderComponents();
            //_mainTabNavigation.ClickProfileTab();
            _mainNavigation.GoToProfile();
            _profilePage.Tabs.GoToSkillsTab();

        }

        public void NavigateToProfile()
        {
            //_mainTabNavigation.RenderComponents();
            //_mainTabNavigation.ClickProfileTab();

            _mainNavigation.GoToProfile();

        }
 

    }
}