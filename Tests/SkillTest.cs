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
    public class SkillTests : TestBase
    {
    
        private SkillSteps _steps;
        private SkillPage _skillPage; 

        [SetUp]
        public void Setup()
        {
         
            _steps = new SkillSteps(Driver, State);
            _skillPage = new SkillPage(Driver, State);

        }

        [Test, Category("Skill")]
        public void GivenValidSkill_WhenAdd_ThenSuccess()
        {
            _steps.GivenIHaveSkillData("validSkill");
            _steps.WhenIAddTheSkillRecord();

            AssertionHelper.AssertToastMessage(_skillPage.GetActualMessage(), State.CurrentSkill.ExpectedMessage);

            AssertionHelper.AssertRecordPresent(_skillPage.IsRecordPresent(), "Skill");

        }
        [Test,Category("Skill")]
        public void GivenEmptySkill_WhenAdd_ThenFail()
        {
            _steps.GivenIHaveSkillData("emptySkill");
            _steps.WhenIAddTheSkillRecord();

            AssertionHelper.AssertToastMessage(_skillPage.GetActualMessage(), State.CurrentSkill.ExpectedMessage);

            AssertionHelper.AssertRecordNotPresent(_skillPage.IsRecordPresent(), "Skill");
        }

        [Test, Category("Skill")]
        public void GivenEmptyLevel_WhenAdd_ThenFail()
        {
            _steps.GivenIHaveSkillData("emptyLevel");
            _steps.WhenIAddTheSkillRecord();

            AssertionHelper.AssertToastMessage(_skillPage.GetActualMessage(), State.CurrentSkill.ExpectedMessage);

           AssertionHelper.AssertRecordNotPresent(_skillPage.IsRecordPresent(), "Skill");
        }

        [Test, Category("Skill")]
        public void GivenDuplicateSkill_WhenAdd_ThenFail()
        {
            _steps.GivenIHaveSkillData("validSkill");
            _steps.WhenIAddTheSkillRecord();

            _steps.GivenIHaveSkillData("duplicateSkill");
            _steps.WhenIAddTheSkillRecord();

            AssertionHelper.AssertToastMessage(_skillPage.GetActualMessage(), State.CurrentSkill.ExpectedMessage);
            
        }


        [Test, Category ("Skill")]
        public void GivenValidSkill_WhenEdit_ThenSuccess() 
        {
            _steps.GivenIHaveSkillData("validSkill");
            _steps.WhenIAddTheSkillRecord();

            _steps.WhenIEditTheSkillRecord("editSkill");

            AssertionHelper.AssertToastMessage(_skillPage.GetActualMessage(), State.CurrentSkill.ExpectedMessage);

            AssertionHelper.AssertRecordPresent(_skillPage.IsRecordPresent(), "Skill");



        }

        [Test, Category("Skill")]
        public void GivenEmptySkill_WhenEdit_ThenFail()
        {
            _steps.GivenIHaveSkillData("validSkill");
            _steps.WhenIAddTheSkillRecord();

            _steps.WhenIEditTheSkillRecord("emptySkill");

            AssertionHelper.AssertToastMessage(_skillPage.GetActualMessage(), State.CurrentSkill.ExpectedMessage);
            
        }

        [Test, Category("Skill")]
        public void GivenValidSkill_WhenEditDuplicateRecord_ThenFail()
        {
            _steps.GivenIHaveSkillData("validSkill");
            _steps.WhenIAddTheSkillRecord();

            _steps.GivenIHaveSkillData("validSkill2");
            _steps.WhenIAddTheSkillRecord();

            _steps.WhenIEditTheSkillRecord("duplicateSkill");

            AssertionHelper.AssertToastMessage(_skillPage.GetActualMessage(), State.CurrentSkill.ExpectedMessage);

        }

        [Test, Category("Skill")]
        public void GivenValidSkill_WhenDelete_ThenSuccess()
        {
            _steps.GivenIHaveSkillData("validSkill");
            _steps.WhenIAddTheSkillRecord();

            _steps.WhenDeleteTheSkillRecord("deleteSkill");

            AssertionHelper.AssertToastMessage(_skillPage.GetActualMessage(), State.CurrentSkill.ExpectedMessage);

        
        }

      
    }

}
