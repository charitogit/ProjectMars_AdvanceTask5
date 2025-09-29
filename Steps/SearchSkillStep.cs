using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.Pages.SearchSkill;
using ProjectMars_AdvanceTask_NUnit.Pages.Shared;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Steps
{
    public class SearchSkillStep
    {
        private readonly IWebDriver _driver;
        private readonly TestStateInfo _state;
        private readonly SearchSkillPage _searchSkillPage;

        private MainNavigationComponent _mainNavigationComponent;
        public SearchSkillStep(IWebDriver driver, TestStateInfo state)
        {
            _driver = driver;
            _state = state;
            _searchSkillPage = new SearchSkillPage(_driver, _state);
            _mainNavigationComponent = new MainNavigationComponent(_driver);
        }

        public void GivenIHaveSearchSkillData(string dataKey)
        {
            _state.CurrentSearchSkill = TestDataHelper.GetSearchSkillData(dataKey)
                ?? throw new InvalidOperationException($"No Search Skill data found for key: {dataKey}");

            if (string.IsNullOrWhiteSpace(_state.CurrentSearchSkill.Keyword))
                throw new ArgumentException($"Keyword is missing for Search Skill data key: {dataKey}");
        }
        public void NavigateToSearchSkill()
        {
            _mainNavigationComponent.ClickSearchSkill();
        }
        public void WhenISearchForSkillByAllCategory()
        {
            NavigateToSearchSkill();
            _searchSkillPage.DoSearchByAllCategory();
        }

        public void WhenISearchForSkillByAllCategoryWithFilterLocationType()
        {
            NavigateToSearchSkill();
            _searchSkillPage.DoSearchByAllCategory();
            _searchSkillPage.ClickFilterLocationTypeButton();
        }

        public void WhenISearchForSkillBySubCategory()
        {
            NavigateToSearchSkill();
            _searchSkillPage.DoSearchBySubCategory();

        }
    }
}
