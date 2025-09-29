using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Pages.Components;
using ProjectMars_AdvanceTask_NUnit.Pages.Dashboard;
using ProjectMars_AdvanceTask_NUnit.Pages.ManageListings;
using ProjectMars_AdvanceTask_NUnit.Pages.Shared;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using System.Collections;
using System.Diagnostics;

public class CleanupManager
{
    private readonly IWebDriver _driver;
    private readonly TestStateInfo _state;

    protected NavigationHelper _navigationHelper;
    protected LanguagePage _languagePage;   
    protected SkillPage _skillPage;
    protected MainNavigationComponent _mainNavigation;
    protected ListingManagement _listingManagement;
    protected NotificationPage _notificationPage;

    public CleanupManager(IWebDriver driver, TestStateInfo state)
    {
        _driver = driver;
        _state = state;

        _navigationHelper = new NavigationHelper(_driver);
        _languagePage = new LanguagePage(_driver, _state);  
        _skillPage = new SkillPage(_driver, _state);
        _mainNavigation = new MainNavigationComponent(_driver);
        _listingManagement = new ListingManagement(_driver, _state);
        _notificationPage = new NotificationPage(_driver);

    }

    public void RunPreTestCleanup(IEnumerable categories)
    {
        // Convert to list of strings
        var categoryList = categories.Cast<string>().ToList();

        if (categoryList.Contains("Language"))
            CleanupLanguage();

        if (categoryList.Contains("Skill"))
            CleanupSkill();

        if (categoryList.Contains("ShareSkill") || categoryList.Contains("SearchSkill"))
            CleanupShareSkill();

        if (categoryList.Contains("Notification"))
        {
           
            CleanupNotification();
            
        }
    }

    // Pre Cleanup Methods  
    private void CleanupLanguage()
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            _languagePage.DeleteAllLanguage();
            stopwatch.Stop();
            Console.WriteLine($"[Hook:Cleanup] Language records cleaned in {stopwatch.Elapsed.TotalSeconds} sec");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Hook:Cleanup] Failed to clean Language: " + ex.Message);
        }
    }

    private void CleanupSkill()
    {
        try
        {
           
            _navigationHelper.NavigateToSkillTab();

            var stopwatch = Stopwatch.StartNew();
            _skillPage.DeleteAllSkill();
            stopwatch.Stop();
            Console.WriteLine($"[Hook:Cleanup] Skill records cleaned in {stopwatch.Elapsed.TotalSeconds} sec");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Hook:Cleanup] Failed to clean Skill: " + ex.Message);
        }
    }

    private void CleanupShareSkill()
    {
        try
        {
            
            _mainNavigation.GoToManageListings();

            var stopwatch = Stopwatch.StartNew();
            _listingManagement.DeleteAllSkillListing();
            stopwatch.Stop();
            Console.WriteLine($"[Hook:Cleanup] ShareSkill records cleaned in {stopwatch.Elapsed.TotalSeconds} sec");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Hook:Cleanup] Failed to clean ShareSkill: " + ex.Message);
        }
    }

    private void CleanupNotification()
    {
        try
        {
            _mainNavigation.GoToDashboard();
            var stopwatch = Stopwatch.StartNew();
            _notificationPage.DeleteAllNotifications();
            stopwatch.Stop();

            Console.WriteLine($"[Hook:Cleanup] Notifications cleaned in {stopwatch.Elapsed.TotalSeconds} sec");
        }
        catch(Exception ex)
        {
            Console.WriteLine("[Hook:Cleanup] Failed to clean Notifications: " + ex.Message);
        }   
    }

    //Post Cleanup Action
    public void RunPostTestCleanup(IEnumerable categories)
    {
        // Convert to list of strings
        var categoryList = categories.Cast<string>().ToList();

        if (categoryList.Contains("Language"))
        {
            CleanupLanguageAfterTest();
        }

        if (categoryList.Contains("Skill"))
        {
            CleanupSkillAfterTest();
        }

        if (categoryList.Contains("ShareSkill") || categoryList.Contains("SearchSkill"))
            CleanupShareSkillAfterTest();


    }

    //Post Cleanup Methods

    private void CleanupLanguageAfterTest()
    {
        try
        {

            // Check if table has any rows before continuing
            var rowsXPath = "//div[@data-tab='first' and contains(@class,'active')]//table/tbody/tr";
            var existingRows = _driver.FindElements(By.XPath(rowsXPath));

            if (existingRows.Count == 0)
            {
                Console.WriteLine("[Hook:AfterScenario] Language table is already empty. Skipping cleanup.");
                return;
            }

            if (_state.LanguageDataList != null && _state.LanguageDataList.Any())
            {
                foreach (var language in _state.LanguageDataList)
                {
                    _languagePage.DeleteLanguageIfExists(language);
                }

                Console.WriteLine($"[Hook:TearDown] Deleted {_state.LanguageDataList.Count} language records.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[AfterScenario Language Cleanup ERROR] " + ex.Message);
        }
    }

    private void CleanupSkillAfterTest()
    {
        try
        {

            // Check if table has any rows before continuing
            var rowsXPath = "//div[@data-tab='second' and contains(@class,'active')]//table/tbody/tr";
            var existingRows = _driver.FindElements(By.XPath(rowsXPath));

            if (existingRows.Count == 0)
            {
                Console.WriteLine("[Hook:AfterScenario] Skill table is already empty. Skipping cleanup.");
                return;
            }

            if (_state.SkillDataList != null && _state.SkillDataList.Any())
            {
                foreach (var skill in _state.SkillDataList)
                {
                    _skillPage.DeleteSkillIfExists(skill);
                }

                Console.WriteLine($"[Hook:TearDown] Deleted {_state.SkillDataList.Count} skill records.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[AfterScenario Skill Cleanup ERROR] " + ex.Message);
        }
    }

    private void CleanupShareSkillAfterTest()
    {
        try
        {
            if (_state.ShareSkillDataList.Any())
            {
                _mainNavigation.GoToManageListings();

                var stopwatch = Stopwatch.StartNew();
                _listingManagement.DeleteSpecificShareSkillListings(_state.ShareSkillDataList);
                stopwatch.Stop();

                Console.WriteLine($"[Hook:Cleanup] Deleted {_state.ShareSkillDataList.Count} ShareSkill records in {stopwatch.Elapsed.TotalSeconds} sec");

                // Keep state clean
                _state.ShareSkillDataList.Clear();
            }
            else
            {
                Console.WriteLine("[Hook:Cleanup] No ShareSkill records in state. Skipping cleanup.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Hook:Cleanup] Failed to clean ShareSkill: " + ex.Message);
        }
    }


}
