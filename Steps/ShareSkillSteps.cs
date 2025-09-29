using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.Pages;
using ProjectMars_AdvanceTask_NUnit.Pages.Components;
using ProjectMars_AdvanceTask_NUnit.Pages.ManageListings;
using ProjectMars_AdvanceTask_NUnit.Pages.Shared;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Steps
{
    public class ShareSkillSteps
    {
        private  IWebDriver _driver;
        private readonly TestStateInfo _state;
        private readonly ShareSkillPage _shareSkillPage;
        private MainNavigationComponent _mainNavigationComponent;
       
        private SingleFieldTestData _title;
        private SingleFieldTestData _description;
        private SingleFieldTestData _tags;


        private String _expectedMessage;

        public String Title => _title.Value; // Expose title for assertions
        public String Description => _description.Value; // Expose description for assertions
        public String ExpectedMessage => _expectedMessage; // Expose expected message for assertions

   
        public ShareSkillSteps(IWebDriver driver, TestStateInfo state)
        {
            _driver = driver; 
             _state = state;

            _mainNavigationComponent = new MainNavigationComponent(_driver);
            _shareSkillPage = new ShareSkillPage(_driver,_state);
         
        }
        
        public void NavigateToShareSkill()
        {
            _mainNavigationComponent.ClickShareSkill();
        }

        public void GivenIHaveShareSkillData(string dataKey)
        {
            _state.CurrentShareSkill = GetShareSkillData(dataKey);

            if (_state.CurrentShareSkill == null)
                throw new InvalidOperationException($"No Share Skill data found for key: {dataKey}");

        }

    
        public void GivenIHaveSpecialCharacterTitle(string folder,string dataKey)
        {
             _title= TestDataHelper.GetSingleFieldTestData(folder, dataKey);
            _expectedMessage = _title.ExpectedMessage; 
        }
        public void GivenIHaveSpecialCharacterDescription(string folder,string dataKey)
        {
            _description=TestDataHelper.GetSingleFieldTestData(folder,dataKey);
            _expectedMessage=_description.ExpectedMessage;
        }
        public void GivenIHaveMoreThan1000Characters(string folder, string dataKey)
        {
            _title = TestDataHelper.GetSingleFieldTestData(folder, dataKey);
            _description = TestDataHelper.GetSingleFieldTestData(folder, dataKey);
            
        }


        public void GivenIHaveDuplicateTags(string folder, string dataKey)
        {
            _tags = TestDataHelper.GetSingleFieldTestData(folder, dataKey);
          
        }


        public void WhenISubmitTheShareSkillRecord()
        {
            NavigateToShareSkill();

            _shareSkillPage.FillAndSubmitShareSkill();

            //for after test cleanup
          
            if (!_state.ShareSkillDataList.Contains(_state.CurrentShareSkill))
            {
                _state.ShareSkillDataList.Add(_state.CurrentShareSkill);

                Console.WriteLine($"[Step] Adding submitted share skill to data list for post cleanup: {_state.CurrentShareSkill.Title} and {_state.CurrentShareSkill.Description}");
            }

        }

        public void WhenIEnterTitle()
        {
            NavigateToShareSkill();
            _shareSkillPage.FillAndEnterTitle(_title.Value);
        }
        public void WhenIEnterDescription()
        {
            NavigateToShareSkill();
            _shareSkillPage.FillAndEnterDescription(_description.Value);
        }

        public void WhenIEnterTags()
        {
            NavigateToShareSkill();
            _shareSkillPage.FillAndEnterTags(_tags.Values);
        }
        public void WhenIEnterSkillExchangeTags()
        {
            NavigateToShareSkill();
            _shareSkillPage.FillAndEnterSkillExchangeTags(_tags.Values);
        }

        public void WhenIRemoveAllTags()
        {
           _shareSkillPage.RemoveAllTags();
        }
       


        //Private utility method to centralize fetching of share skill data from deserialization
        private ShareSkillModel GetShareSkillData(string dataKey)
        {
            var data = TestDataHelper.GetShareSkillData(dataKey);
            Console.WriteLine($"Loaded Share Skill Data: {data.Title}, {data.Description},{data.Category},{data.SubCategory},{data.Tags},{data.ServiceType},{data.LocationType},{data.ServiceType},{data.Status}");

            if (data == null)
                throw new Exception($"Share Skill data for key '{dataKey}' is null. Check JSON or TestDataHelper logic.");
            return data;
        }

    }
}
