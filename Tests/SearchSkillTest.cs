using NUnit.Framework;
using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Hooks;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.Steps;
using ProjectMars_AdvanceTask_NUnit.Pages.SearchSkill;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMars_AdvanceTask_NUnit.Helpers;

namespace ProjectMars_AdvanceTask_NUnit.Tests
{
    [TestFixture]
    public class SearchSkillTest : TestBase
    {
        private ShareSkillSteps _shareSkillStep; 
        private SearchSkillStep _searchSkillStep;
        private SearchSkillPage _searchSkillPage;

 
        [SetUp]
        public void SetUp()
        {
            _shareSkillStep = new ShareSkillSteps(Driver, State);
            _searchSkillStep = new SearchSkillStep(Driver,State);
            _searchSkillPage = new SearchSkillPage(Driver,State);

            EnsureShareSkillExists("validShareSkill1");
            EnsureShareSkillExists("validShareSkill2");
            EnsureShareSkillExists("validShareSkill3");
            EnsureShareSkillExists("validShareSkill4");
        }

        [Test, Category("SearchSkill")]
        public void GivenMultipleShareSkillsExist_WhenSearchingByTitleAndAllCategories_ThenResultsAreReturned
()
        {
            _searchSkillStep.GivenIHaveSearchSkillData("AllCategory_Title");
            _searchSkillStep.WhenISearchForSkillByAllCategory();

            AssertionHelper.AssertCountEqual(_searchSkillPage.GetAllCategoryOptionCount(), State.CurrentSearchSkill.ExpectedCount, "All Category Option");
            AssertionHelper.AssertRecordPresent(_searchSkillPage.AreAllCardsContainingKeyword(State.CurrentSearchSkill.Keyword), $"Keyword {State.CurrentSearchSkill.Keyword}");

            Thread.Sleep(3000);
        }

        [Test, Category("SearchSkill")]
        public void GivenMultipleShareSkillsExist_WhenSearchingByDescriptionAndAllCategories_ThenResultsAreReturned
()
        {
            _searchSkillStep.GivenIHaveSearchSkillData("AllCategory_Description");
            _searchSkillStep.WhenISearchForSkillByAllCategory();

            AssertionHelper.AssertCountEqual(_searchSkillPage.GetAllCategoryOptionCount(), State.CurrentSearchSkill.ExpectedCount, "All Category Option");
            AssertionHelper.AssertRecordPresent(_searchSkillPage.AreAllCardsContainingKeyword(State.CurrentSearchSkill.Keyword), $"Keyword {State.CurrentSearchSkill.Keyword}");

            Thread.Sleep(3000);
        }

        [Test, Category("SearchSkill")]
        public void GivenMultipleShareSkillsExist_WhenSearchingByTagAndAllCategories_ThenResultsAreReturned
()
        {
            _searchSkillStep.GivenIHaveSearchSkillData("AllCategory_Tag");
            _searchSkillStep.WhenISearchForSkillByAllCategory();

            AssertionHelper.AssertCountEqual(_searchSkillPage.GetAllCategoryOptionCount(), State.CurrentSearchSkill.ExpectedCount, "All Category Option");
            AssertionHelper.AssertRecordPresent(_searchSkillPage.AreAllCardsContainingKeyword(State.CurrentSearchSkill.Keyword), $"Keyword {State.CurrentSearchSkill.Keyword}");

            Thread.Sleep(3000);
        }

        [Test, Category("SearchSkill")]
        public void GivenMultipleShareSkillsExist_WhenISearchBySubCategory_ThenResultsAreReturned()
        {
            
            _searchSkillStep.GivenIHaveSearchSkillData("SubCategory_Tag");
            _searchSkillStep.WhenISearchForSkillBySubCategory();

            Thread.Sleep(3000);
            
            AssertionHelper.AssertCountEqual(_searchSkillPage.GetSubCategoryOptionCount(), State.CurrentSearchSkill.ExpectedCount, "Sub Category Option");
            AssertionHelper.AssertRecordPresent(_searchSkillPage.AreAllCardsContainingKeyword(State.CurrentSearchSkill.Keyword), $"Keyword {State.CurrentSearchSkill.Keyword}");

        }

        [Test, Category("SearchSkill")]
        public void GivenMultipleShareSkillsExist_WhenISearchByAllCategoryWithFilterOnline_ThenResultsAreReturned()
        {
             
            _searchSkillStep.GivenIHaveSearchSkillData("AllCategory_FilterLocationType_Online");
            _searchSkillStep.WhenISearchForSkillByAllCategoryWithFilterLocationType();
            
            Thread.Sleep(3000);
            AssertionHelper.AssertCountEqual(_searchSkillPage.GetAllCategoryOptionCount(), State.CurrentSearchSkill.ExpectedCount, "All Category Option");
            AssertionHelper.AssertRecordPresent(_searchSkillPage.AreAllCardsContainingKeyword(State.CurrentSearchSkill.Keyword), $"Keyword {State.CurrentSearchSkill.Keyword}");
        }

        [Test, Category("SearchSkill")]
        public void GivenMultipleShareSkillsExist_WhenISearchByAllCategoryWithFilterOnsite_ThenResultsAreReturned()
        {

            _searchSkillStep.GivenIHaveSearchSkillData("AllCategory_FilterLocationType_Onsite");
            _searchSkillStep.WhenISearchForSkillByAllCategoryWithFilterLocationType();

            Thread.Sleep(3000);
            AssertionHelper.AssertCountEqual(_searchSkillPage.GetAllCategoryOptionCount(), State.CurrentSearchSkill.ExpectedCount, "All Category Option");
            AssertionHelper.AssertRecordPresent(_searchSkillPage.AreAllCardsContainingKeyword(State.CurrentSearchSkill.Keyword), $"Keyword {State.CurrentSearchSkill.Keyword}");
        }

        [Test, Category("SearchSkill")]
        public void GivenMultipleShareSkillsExist_WhenISearchByAllCategoryWithFilterShowAll_ThenResultsAreReturned()
        {

            _searchSkillStep.GivenIHaveSearchSkillData("AllCategory_FilterLocationType_ShowAll");
            _searchSkillStep.WhenISearchForSkillByAllCategoryWithFilterLocationType();

            Thread.Sleep(3000);
            AssertionHelper.AssertCountEqual(_searchSkillPage.GetAllCategoryOptionCount(), State.CurrentSearchSkill.ExpectedCount, "All Category Option");
            AssertionHelper.AssertRecordPresent(_searchSkillPage.AreAllCardsContainingKeyword(State.CurrentSearchSkill.Keyword), $"Keyword {State.CurrentSearchSkill.Keyword}");
        }


        [Test, Category("SearchXXSkill")]
        public void GivenMultipleShareSkillsExist_WhenSearchingNonExistingKeyWord_ThenReturnNonExistingResults
()
        {
            _searchSkillStep.GivenIHaveSearchSkillData("NonExistingKeyWord");
            _searchSkillStep.WhenISearchForSkillByAllCategory();

            AssertionHelper.AssertCountEqual(_searchSkillPage.GetAllCategoryOptionCount(), State.CurrentSearchSkill.ExpectedCount, "All Category Option");
            AssertionHelper.AssertToastMessage(_searchSkillPage.GetResultMessage(), State.CurrentSearchSkill.ExpectedMessage);

            Thread.Sleep(3000);
        }


        //private reusable method to ensure share skill exists before running tests

        private void EnsureShareSkillExists(string skillKey)
        {
            _shareSkillStep.GivenIHaveShareSkillData(skillKey);
            _shareSkillStep.WhenISubmitTheShareSkillRecord();
        }

     }
}
