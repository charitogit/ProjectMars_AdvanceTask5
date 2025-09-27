using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Pages.ProfileTabPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Pages.Profile
{
    public class ProfilePage
    {
        private readonly IWebDriver _driver;
        public ProfileTabsComponent Tabs { get; }
        public ProfileDetailComponent Details { get; }

        public ProfilePage(IWebDriver driver)
        {
            _driver = driver;
            Tabs = new ProfileTabsComponent(_driver);
            Details = new ProfileDetailComponent(_driver);
        }
    }

}
