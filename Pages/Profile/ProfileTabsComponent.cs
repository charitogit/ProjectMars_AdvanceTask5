using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Utilities;

namespace ProjectMars_AdvanceTask_NUnit.Pages.ProfileTabPages
{
    public class ProfileTabsComponent
    {
        private readonly IWebDriver _driver;

        private IWebElement SkillsTab;
        private IWebElement LanguagesTab;
        private IWebElement EducationTab;
        private IWebElement CertificationTab;

        public ProfileTabsComponent(IWebDriver driver)
        {
            _driver = driver;
        }


        public void GoToLanguagesTab()
        {
            // Wait and click the "Languages" tab
            string tabXPath = "//*[@id='account-profile-section']//a[contains(text(),'Languages')]";
            Wait.WaitToBeVisible(_driver, "XPath", tabXPath, 10);
            _driver.FindElement(By.XPath(tabXPath)).Click();

            // Wait for the Languages section to be active
            string activeTabContentXPath = "//div[@data-tab='first' and contains(@class, 'active')]";
            Wait.WaitToBeVisible(_driver, "XPath", activeTabContentXPath, 10);
        }

        public void GoToSkillsTab()
        {
            // Wait and click the "Skills" tab
            string tabXPath = "//*[@id='account-profile-section']//a[contains(text(),'Skills')]";
            Wait.WaitToBeVisible(_driver, "XPath", tabXPath, 10);
            _driver.FindElement(By.XPath(tabXPath)).Click();

            // Wait for the Skills section to be active
            string activeTabContentXPath = "//div[@data-tab='second' and contains(@class, 'active')]";
            Wait.WaitToBeVisible(_driver, "XPath", activeTabContentXPath, 10);
        }
        public void GoToEducationTab()
        {
            // Wait and click the "Education" tab
            string tabXPath = "//*[@id='account-profile-section']//a[contains(text(),'Education')]";
            Wait.WaitToBeVisible(_driver, "XPath", tabXPath, 10);
            _driver.FindElement(By.XPath(tabXPath)).Click();

            // Wait for the Education section to be active
            string activeTabContentXPath = "//div[@data-tab='third' and contains(@class, 'active')]";
            Wait.WaitToBeVisible(_driver, "XPath", activeTabContentXPath, 10);
        }

        public void GoToCertificationsTab()
        {
            // Wait and click the "Certifications" tab
            string tabXPath = "//*[@id='account-profile-section']//a[contains(text(),'Certifications')]";
            Wait.WaitToBeVisible(_driver, "XPath", tabXPath, 10);
            _driver.FindElement(By.XPath(tabXPath)).Click();

            // Wait for the Certifications section to be active
            string activeTabContentXPath = "//div[@data-tab='fourth' and contains(@class, 'active')]";
            Wait.WaitToBeVisible(_driver, "XPath", activeTabContentXPath, 10);


        }

    }
}
