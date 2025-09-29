using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.Pages.Components;
using ProjectMars_AdvanceTask_NUnit.Pages.Profile;
using ProjectMars_AdvanceTask_NUnit.TestStates;

namespace ProjectMars_AdvanceTask_NUnit.Steps
{
    public class LanguageSteps
    {
        private IWebDriver _driver;
        private readonly TestStateInfo _state;
        

        private NavigationHelper _navigationHelper;
        private readonly LanguagePage _languagePage;

        public LanguageSteps(IWebDriver driver, TestStateInfo state)
        {
            _driver = driver;

            _state = state;

            _navigationHelper = new NavigationHelper(_driver);
    
            _languagePage = new LanguagePage(_driver,_state);
        }  

        private void NavigateToLanguageTab()
        {
            _navigationHelper.NavigateToLanguageTab(); 
        }


        public void GivenIHaveLanguageData(string dataKey)
        {
          _state.CurrentLanguage = GetLanguageData(dataKey);

            if (_state.CurrentLanguage == null)
                throw new InvalidOperationException($"No Language data found for key: {dataKey}");


        }
      

        public void WhenIAddTheLanguageRecord()
        {
            NavigateToLanguageTab();

            _languagePage.AddLanguage();

            //for after test cleanup
            _state.IsLanguageAdded = true;

            if (!_state.LanguageDataList.Contains(_state.CurrentLanguage))
            {
            _state.LanguageDataList.Add(_state.CurrentLanguage);
                _state.IsLanguageAdded = true; 

             Console.WriteLine($"[Step] Adding Language: {_state.CurrentLanguage.LanguageName}");
            }
            
        }

        public void WhenIEditTheLanguageRecord(string dataKey)
        {
            NavigateToLanguageTab();

            //assign objects from TestState 
            _state.OriginalLanguage = _state.CurrentLanguage;
            _state.NewLanguage = GetLanguageData(dataKey);

            _state.CurrentLanguage = _state.NewLanguage; //for later assertion reference

            //for after test cleanup
            if (_state.IsLanguageAdded ==true && !_state.LanguageDataList.Contains(_state.CurrentLanguage))
            {
                _state.LanguageDataList.Add(_state.CurrentLanguage);
               
                Console.WriteLine($"[Step]  After successful Edit,  Adding language to data list for post cleanup: {_state.CurrentLanguage.LanguageName}");
            }

            _languagePage.EditLanguage(_state.OriginalLanguage);
        }

        public void WhenDeleteTheLanguageRecord(string dataKey)
        {
            NavigateToLanguageTab();

            //assign objects from TestState 
            _state.OriginalLanguage = _state.CurrentLanguage;
            _state.NewLanguage = GetLanguageData(dataKey);

            _state.CurrentLanguage = _state.NewLanguage; //for later assertion reference

            if (_state.LanguageDataList != null && _state.LanguageDataList.Any())
            {
                foreach (var language in _state.LanguageDataList)
                {
                    _languagePage.DeleteLanguageIfExists(language);
                }

                Console.WriteLine($"[Step] Deleted {_state.LanguageDataList.Count} language records.");
            }
        }

     
        //Private utility method to centralize fetching of education data from deserialization
        private Languages GetLanguageData(string dataKey)
        {
            var data = TestDataHelper.GetLanguageData(dataKey);
            Console.WriteLine($"[Step] Loaded Language: {data.LanguageName}, {data.Level}");

            if (data == null)
                throw new Exception($"[Step] Language data for key '{dataKey}' is null. Check JSON or TestDataHelper logic.");
            return data;
        }

    }
}
