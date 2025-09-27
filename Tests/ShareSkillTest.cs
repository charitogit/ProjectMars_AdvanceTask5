using NUnit.Framework;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Hooks;
using ProjectMars_AdvanceTask_NUnit.Pages.Components;
using ProjectMars_AdvanceTask_NUnit.Pages.ManageListings;
using ProjectMars_AdvanceTask_NUnit.Steps;
using ProjectMars_AdvanceTask_NUnit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Tests
{
    [TestFixture]
  
    public class ShareSkillTests : TestBase
    {

        private ShareSkillSteps _steps;
        private ShareSkillPage _shareSkillPage;
        private ListingManagement _listingManagement;

        [SetUp]
        public void Setup()
        {

            _steps = new ShareSkillSteps(Driver, State);
            _shareSkillPage = new ShareSkillPage(Driver, State);
            _listingManagement = new ListingManagement(Driver, State); 

        }


        [Test, Category("ShareSkill")]
        public void GivenValidShareSkill_WhenSubmit_ThenSuccess()
        {
            _steps.GivenIHaveShareSkillData("validShareSkill1");
            _steps.WhenISubmitTheShareSkillRecord();

            _steps.GivenIHaveShareSkillData("validShareSkill2");
            _steps.WhenISubmitTheShareSkillRecord();

            _steps.GivenIHaveShareSkillData("validShareSkill3");
            _steps.WhenISubmitTheShareSkillRecord();

            _steps.GivenIHaveShareSkillData("validShareSkill4");
            _steps.WhenISubmitTheShareSkillRecord();

            Thread.Sleep(3000);
            AssertionHelper.AssertUrlContains(Driver, "Home/ListingManagement", "Manage Listing page where Shared Skill is added");

            AssertionHelper.AssertRecordPresent(_listingManagement.IsRecordPresent(), "Shared Skill");

        }


        [Test, Category("ShareSkill")]
        public void GivenValidShareSkillWithSkillExchange_WhenSubmit_ThenSuccess()
        {
            _steps.GivenIHaveShareSkillData("validShareSkill-SkillExchange");
            _steps.WhenISubmitTheShareSkillRecord();

            // Wait for a known element on the Manage Listing page
            Wait.WaitToBeVisible(Driver, "XPath", "//h2[text()='Manage Listings']", 5);  
            AssertionHelper.AssertUrlContains(Driver, "Home/ListingManagement", "Manage Listing page where Shared Skill is added");

            AssertionHelper.AssertRecordPresent(_listingManagement.IsRecordPresent(), "Shared Skill");

        }
        [Test, Category("ShareSkill")]
        public void GivenValidShareSkillWithCredit_WhenSubmit_ThenSuccess()
        {
            _steps.GivenIHaveShareSkillData("validShareSkill-Credit");
            _steps.WhenISubmitTheShareSkillRecord();

            Thread.Sleep(3000);
            Assert.That(Driver.Url, Does.Contain("Home/ListingManagement"), "Did not navigate to Manage Listing page where Shared Skill added");

            AssertionHelper.AssertRecordPresent(_listingManagement.IsRecordPresent(), "Shared Skill");

        }

        [Test, Category("ShareSkill")]
        public void GivenEmptyTitle_WhenSubmitShareSkilll_ThenFail()
        {
            _steps.GivenIHaveShareSkillData("emptyTitle");
            _steps.WhenISubmitTheShareSkillRecord();

            AssertionHelper.AssertToastMessage(_shareSkillPage.GetActualMessage(), State.CurrentShareSkill.ExpectedMessage);

        }

        [Test, Category("ShareSkill")]
        public void GivenEmptyDescription_WhenSubmitShareSkilll_ThenFail()
        {
            _steps.GivenIHaveShareSkillData("emptyDescription");
            _steps.WhenISubmitTheShareSkillRecord();

            AssertionHelper.AssertToastMessage(_shareSkillPage.GetActualMessage(), State.CurrentShareSkill.ExpectedMessage);

        }

        [Test, Category("ShareSkill")]
        public void GivenEmptyCategory_WhenSubmitShareSkilll_ThenFail()
        {
            _steps.GivenIHaveShareSkillData("emptyCategory");
            _steps.WhenISubmitTheShareSkillRecord();

            AssertionHelper.AssertToastMessage(_shareSkillPage.GetActualMessage(), State.CurrentShareSkill.ExpectedMessage);

        }

        [Test, Category("ShareSkill")]
        public void GivenEmptySubCategory_WhenSubmitShareSkilll_ThenFail()
        {
            _steps.GivenIHaveShareSkillData("emptySubCategory");
            _steps.WhenISubmitTheShareSkillRecord();

            AssertionHelper.AssertToastMessage(_shareSkillPage.GetActualMessage(), State.CurrentShareSkill.ExpectedMessage);

        }

        [Test, Category("ShareSkill")]
        public void GivenEmptyTag_WhenSubmitShareSkilll_ThenFail()
        {
            _steps.GivenIHaveShareSkillData("emptyTag");
            _steps.WhenISubmitTheShareSkillRecord();

            AssertionHelper.AssertToastMessage(_shareSkillPage.GetActualMessage(), State.CurrentShareSkill.ExpectedMessage);

        }

        [Test, Category("ShareSkill")]
        public void GivenEmptySkillExchangeTag_WhenSubmitShareSkilll_ThenFail()
        {
            _steps.GivenIHaveShareSkillData("emptySkillExchangeTag");
            _steps.WhenISubmitTheShareSkillRecord();

            AssertionHelper.AssertToastMessage(_shareSkillPage.GetActualMessage(), State.CurrentShareSkill.ExpectedMessage);

        }

        [Test, Category("ShareSkill")]
        public void GivenSpecialCharactersTitle_WhenEnter_ThenFail()
        {
            _steps.GivenIHaveSpecialCharacterTitle("ShareSkill", "specialCharacters");
            _steps.WhenIEnterTitle();

            AssertionHelper.AssertToastMessage(_shareSkillPage.GetActualMessage(), _steps.ExpectedMessage);

        }

        [Test, Category("ShareSkill")]
        public void GivenSpecialCharactersDescription_WhenEnter_ThenFail()
        {
            _steps.GivenIHaveSpecialCharacterDescription("ShareSkill", "specialCharacters"); 
            _steps.WhenIEnterDescription();

             AssertionHelper.AssertToastMessage(_shareSkillPage.GetActualMessage(), _steps.ExpectedMessage);


        }

        [Test, Category("ShareXSkill")]
        public void GivenMoreThan1000CharactersTitle_WhenEnter_ThenFail()
        {
            _steps.GivenIHaveMoreThan1000Characters("ShareSkill", "moreThan1000Characters");
            _steps.WhenIEnterTitle();

            Assert.That(_shareSkillPage.GetActualEnteredTitle(), Is.Not.EqualTo(_steps.Title));  
        }

        [Test, Category("ShareXSkill")]
        public void GivenMoreThan1000Description_WhenEnter_ThenFail()
        {
            _steps.GivenIHaveMoreThan1000Characters("ShareSkill", "moreThan1000Characters");
            _steps.WhenIEnterDescription();

            Assert.That(_shareSkillPage.GetActualEnteredDescription(), Is.Not.EqualTo(_steps.Description));
        }

        [Test, Category("SharexSkill")]
        public void GivenDuplicateTags_WhenEnter_ThenFail()
        {
            _steps.GivenIHaveDuplicateTags("ShareSkill", "duplicateTags");
            _steps.WhenIEnterTags();

           Assert.That(_shareSkillPage.TagIsInEditMode(),"Tag is not in edit mode, which indicates a failure in handling duplicate tags.");
           
        }

        [Test, Category("SharexSkill")]
        public void GivenDuplicateSkillExchangeTags_WhenEnter_ThenFail()
        {
            _steps.GivenIHaveDuplicateTags("ShareSkill", "duplicateTags");
            _steps.WhenIEnterSkillExchangeTags();

            Assert.That(_shareSkillPage.SkilllExchangeTagIsInEditMode(), "Tag is not in edit mode, which indicates a failure in handling duplicate tags.");


        }

        [Test, Category("SharexSkill")]
        public void GivenMultipleTags_WhenRemove_ThenSuccess()
        {
            _steps.GivenIHaveDuplicateTags("ShareSkill", "multipleTags");
            _steps.WhenIEnterTags();

            _steps.WhenIRemoveAllTags(); 
             Assert.That(_shareSkillPage.AreAllTagsRemoved(), Is.False, "Not all tags were removed successfully.");
        }

        [Test, Category("SharexSkill")]
        public void GivenMultipleSkillExchangeTags_WhenRemove_ThenSuccess()
        {
            _steps.GivenIHaveDuplicateTags("ShareSkill", "multipleTags");
            _steps.WhenIEnterSkillExchangeTags();

            _steps.WhenIRemoveAllTags();
            Assert.That(_shareSkillPage.AreAllSkillExchangeTagsRemoved(), Is.False, "Not all tags were removed successfully.");
        }

    }





}
