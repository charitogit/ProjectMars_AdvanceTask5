using NUnit.Framework;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Hooks;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.Pages;
using ProjectMars_AdvanceTask_NUnit.Pages.Components;
using ProjectMars_AdvanceTask_NUnit.Pages.Profile;
using ProjectMars_AdvanceTask_NUnit.Steps;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Tests
{
    [TestFixture]
   public class LanguageTests : TestBase
    {
    
        private LanguageSteps _steps;
        private LanguagePage  _languagePage;

        [SetUp]
        public void Setup()
        {
         
            _steps = new LanguageSteps(Driver, State);
            _languagePage =  new LanguagePage(Driver, State);

        }

        [Test, Category("Language")]
        public void GivenValidLanguage_WhenAdd_ThenSuccess()
        {
            _steps.GivenIHaveLanguageData("validLanguage");
            _steps.WhenIAddTheLanguageRecord(); 

            AssertionHelper.AssertToastMessage(_languagePage.GetActualMessage(),State.CurrentLanguage.ExpectedMessage);
            
            AssertionHelper.AssertRecordPresent(_languagePage.IsRecordPresent(), "Language"); 
        }
        [Test,Category("Language")]
        public void GivenEmptyLanguage_WhenAdd_ThenFail()
        {
            _steps.GivenIHaveLanguageData("emptyLanguage");
            _steps.WhenIAddTheLanguageRecord();

            AssertionHelper.AssertToastMessage(_languagePage.GetActualMessage(), State.CurrentLanguage.ExpectedMessage);

            AssertionHelper.AssertRecordNotPresent(_languagePage.IsRecordPresent(), "Language");

        }

        [Test, Category("Language")]
        public void GivenEmptyLevel_WhenAdd_ThenFail()
        {
            _steps.GivenIHaveLanguageData("emptyLevel");
            _steps.WhenIAddTheLanguageRecord();

            AssertionHelper.AssertToastMessage(_languagePage.GetActualMessage(), State.CurrentLanguage.ExpectedMessage);
           
            AssertionHelper.AssertRecordNotPresent(_languagePage.IsRecordPresent(), "Language");

        }

        [Test, Category("Language")]
        public void GivenDuplicateLanguage_WhenAdd_ThenFail()
        {
            _steps.GivenIHaveLanguageData("validLanguage");
            _steps.WhenIAddTheLanguageRecord();

            _steps.GivenIHaveLanguageData("duplicateLanguage");
            _steps.WhenIAddTheLanguageRecord();

            AssertionHelper.AssertToastMessage(_languagePage.GetActualMessage(), State.CurrentLanguage.ExpectedMessage);

        }

        [Test, Category ("Language")]
        public void GivenValidLanguage_WhenEdit_ThenSuccess() 
        {
            _steps.GivenIHaveLanguageData("validLanguage");
            _steps.WhenIAddTheLanguageRecord();

            _steps.WhenIEditTheLanguageRecord("editLanguage");

            AssertionHelper.AssertToastMessage(_languagePage.GetActualMessage(), State.CurrentLanguage.ExpectedMessage);

            AssertionHelper.AssertRecordPresent(_languagePage.IsRecordPresent(), "Language");



        }

        [Test, Category("Language")]
        public void GivenEmptyLanguage_WhenEdit_ThenFail()
        {
            _steps.GivenIHaveLanguageData("validLanguage");
            _steps.WhenIAddTheLanguageRecord();

            _steps.WhenIEditTheLanguageRecord("emptyLanguage");

            AssertionHelper.AssertToastMessage(_languagePage.GetActualMessage(), State.CurrentLanguage.ExpectedMessage);

        }

        [Test, Category("Language")]
        public void GivenValidLanguage_WhenEditDuplicateRecord_ThenFail()
        {
            _steps.GivenIHaveLanguageData("validLanguage");
            _steps.WhenIAddTheLanguageRecord();

            _steps.GivenIHaveLanguageData("validLanguage2");
            _steps.WhenIAddTheLanguageRecord();

            _steps.WhenIEditTheLanguageRecord("duplicateLanguage");

            AssertionHelper.AssertToastMessage(_languagePage.GetActualMessage(), State.CurrentLanguage.ExpectedMessage);

        }

        [Test, Category("Language")]
        public void GivenValidLanguage_WhenDelete_ThenSuccess()
        {
            _steps.GivenIHaveLanguageData("validLanguage");
            _steps.WhenIAddTheLanguageRecord();

            _steps.WhenDeleteTheLanguageRecord("deleteLanguage");

            AssertionHelper.AssertToastMessage(_languagePage.GetActualMessage(), State.CurrentLanguage.ExpectedMessage);

          


        }

    }

}
