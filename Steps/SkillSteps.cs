using NUnit.Framework;
using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Hooks;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.Pages;
using ProjectMars_AdvanceTask_NUnit.Pages.Components;
using ProjectMars_AdvanceTask_NUnit.Pages.Profile;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Steps
{
    public class SkillSteps
    {
        private readonly SkillPage _skillPage;
        private readonly TestStateInfo _state;
        private IWebDriver _driver;

        private NavigationHelper _navigationHelper;
        private ProfilePage _profilePage;

        public SkillSteps(IWebDriver driver, TestStateInfo state)
        {
            _driver = driver;

            _state = state;

            _navigationHelper = new NavigationHelper(_driver);
    
            _skillPage = new SkillPage(_driver,_state);
           
        }  

        private void NavigateToSkillTab()
        {
            _navigationHelper.NavigateToSkillTab(); 
        }


        public void GivenIHaveSkillData(string dataKey)
        {
          _state.CurrentSkill= GetSkillData(dataKey);

            if (_state.CurrentSkill == null)
                throw new InvalidOperationException($"[Step] No skill data found for key: {dataKey}");


        }
      

        public void WhenIAddTheSkillRecord()
        {
            NavigateToSkillTab();

            _skillPage.AddSkill();

            //for after test cleanup
            _state.IsSkillAdded = true; // applicable for successfully edited record w/c also needs to be cleanup post test

            if (!_state.SkillDataList.Contains(_state.CurrentSkill))
            {
            _state.SkillDataList.Add(_state.CurrentSkill);

             Console.WriteLine($"[Step] Adding skill to data list for post cleanup: {_state.CurrentSkill.SkillName} and {_state.CurrentSkill.Level}");
            }
            
        }

        public void WhenIEditTheSkillRecord(string dataKey)
        { 
           NavigateToSkillTab();

            //assign objects from TestState 
            _state.OriginalSkill = _state.CurrentSkill;
            _state.NewSkill = GetSkillData(dataKey);

            _state.CurrentSkill = _state.NewSkill; //for later assertion reference

            //for after test cleanup
            if (_state.IsSkillAdded == true  &&   !_state.SkillDataList.Contains(_state.CurrentSkill))
            {
                _state.SkillDataList.Add(_state.CurrentSkill);

                Console.WriteLine($"[Step] After successful Edit,  Adding skill to data list for post cleanup: {_state.CurrentSkill.SkillName} and {_state.CurrentSkill.Level}");
            }

            _skillPage.EditSkill(_state.OriginalSkill); 
        }

        public void WhenDeleteTheSkillRecord(string dataKey)
        {
            NavigateToSkillTab();

            //assign objects from TestState 
            _state.OriginalSkill = _state.CurrentSkill;
            _state.NewSkill = GetSkillData(dataKey);

            _state.CurrentSkill = _state.NewSkill; //for later assertion reference

            if (_state.SkillDataList != null && _state.SkillDataList.Any())
            {
                foreach (var skill in _state.SkillDataList)
                {
                    _skillPage.DeleteSkillIfExists(skill);
                }

                Console.WriteLine($"[Cleanup] Deleted {_state.SkillDataList.Count} skill records.");
            }
        }

      

        //Private utility method to centralize fetching of education data from deserialization
        private Skill GetSkillData(string dataKey)
        {
            var data = TestDataHelper.GetSkillData(dataKey);
            Console.WriteLine($"[Step] Loaded Skill: {data.SkillName}, {data.Level}");

            if (data == null)
                throw new Exception($"[Step] Skill data for key '{dataKey}' is null. Check JSON or TestDataHelper logic.");
            return data;
        }

    }
}
