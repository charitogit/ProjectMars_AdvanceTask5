using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.Pages.Components;
using ProjectMars_AdvanceTask_NUnit.Pages.Profile;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Steps
{
   
    public class ProfileDetailSteps
    {
        private NavigationHelper _navigationHelper;
        private readonly ProfilePage _profilePage;

        private SingleFieldTestData _availabilityData;
        private SingleFieldTestData _hoursData;
        private SingleFieldTestData _earnTargetData;

        private  string _expectedMessage;


        public ProfileDetailSteps(IWebDriver driver )
        {
            _navigationHelper = new NavigationHelper(driver);
            _profilePage = new ProfilePage(driver);
                    
        }

        public void NavigateToProfile()
        {
            _navigationHelper.NavigateToProfile();
            
        }

        public void GivenIHaveAvailability(string folder,string dataKey)
        {
            _availabilityData = TestDataHelper.GetSingleFieldTestData(folder,dataKey);
            _expectedMessage = _availabilityData.ExpectedMessage;

        }
        public void GivenIHaveHours(string folder, string dataKey)
        {
            _hoursData = TestDataHelper.GetSingleFieldTestData(folder, dataKey);
            _expectedMessage = _hoursData.ExpectedMessage;
        }

        public void GivenIHaveEarnTarget(string folder, string dataKey)
        {
            _earnTargetData = TestDataHelper.GetSingleFieldTestData(folder, dataKey);
            _expectedMessage = _earnTargetData.ExpectedMessage;
        }


        public void SetAvailability()
        {
       
            _profilePage.Details.SelectAvailability(_availabilityData.Value);

        }

        public void SetHours()
        {
            _profilePage.Details.SelectHours(_hoursData.Value);
        }

        public void SetEarnTarget()
        {
            _profilePage.Details.SelectEarnTarget(_earnTargetData.Value); 
        }
        
        //Assertion related logic

        public string GetExpectedMessage()
        {
            return _expectedMessage;
        }
 
        public string GetAvailabilityData()
        {
            
            return _availabilityData.Value;
        }
        public string GetHoursData()
        {
            return _hoursData.Value;
        }
        public string GetEarnTargetData()
        {
            return _earnTargetData.Value;
        }

        public string GetActualMessage()
        {
            return _profilePage.Details.GetActualMessage();
        }

      public string GetActualAvailability()
        {
    
            return _profilePage.Details.GetActualAvailability(_availabilityData.Value);
        }
        public string GetActualHours()
        {
            return _profilePage.Details.GetActualHours(_hoursData.Value);
        }
        public string GetActualEarnTarget()
        {
            return _profilePage.Details.GetActualEarnTarget(_earnTargetData.Value);
        }

    }

}
