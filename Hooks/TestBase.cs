using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ProjectMars_AdvanceTask_NUnit.Config;
using ProjectMars_AdvanceTask_NUnit.Helpers;
using ProjectMars_AdvanceTask_NUnit.Models;
using ProjectMars_AdvanceTask_NUnit.Pages;
using ProjectMars_AdvanceTask_NUnit.Pages.Components;
using ProjectMars_AdvanceTask_NUnit.Pages.ManageListings;
using ProjectMars_AdvanceTask_NUnit.Pages.Shared;
using ProjectMars_AdvanceTask_NUnit.Steps;
using ProjectMars_AdvanceTask_NUnit.TestStates;
using ProjectMars_AdvanceTask_NUnit.Utilities;
using System.Diagnostics;
using System.Text.Json;

namespace ProjectMars_AdvanceTask_NUnit.Hooks
{
    public class TestBase
    {
        protected IWebDriver Driver;
        protected TestStateInfo State;
        protected NavigationHelper NavigationHelper;
        protected SignInPage SignInPage;
        protected SignInSteps SignInSteps;
        protected CleanupManager Cleanup;


        // Static because shared for entire test run
        public static ExtentReports Extent;
        public static ExtentSparkReporter HtmlReporter;
        public static TestSettings Settings;

        // Instance per test, so parallel tests get separate ExtentTest objects
        protected ExtentTest? CurrentTest;

      

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            //Report Setup
            try
            {
                string currentDir = Directory.GetCurrentDirectory();
                string settingsPath = Path.Combine(currentDir, "settings.json");

                if (!File.Exists(settingsPath))
                    throw new FileNotFoundException($"settings.json not found at: {settingsPath}");

                string json = File.ReadAllText(settingsPath);
                Settings = JsonSerializer.Deserialize<TestSettings>(json)
                    ?? throw new Exception("Failed to deserialize settings.json");

                // Setup report path
                string projectRoot = Path.GetFullPath(Path.Combine(currentDir, "..", ".."));
                string reportFileName = Settings.Report.Path.TrimStart('/');
                string reportPath = Path.Combine(projectRoot, reportFileName);

                HtmlReporter = new ExtentSparkReporter(reportPath);
                Extent = new ExtentReports();
                Extent.AttachReporter(HtmlReporter);

                Extent.AddSystemInfo("Environment", Settings.Environment.BaseUrl);
                Extent.AddSystemInfo("Browser", Settings.Browser.Type);

                Console.WriteLine($"[ExtentReport Init] Path: {reportPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ExtentReport Error] {ex.Message}");
                throw;
            }
         
        }

      

        [SetUp]
        public void TestSetup()
        {
            // Initialize ChromeDriver
            var options = new ChromeOptions();
            if (Settings.Browser.Headless)
                options.AddArgument("--headless");

            Driver = new ChromeDriver(options);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Settings.Browser.TimeoutSeconds);
            Driver.Manage().Window.Maximize();


            // Initialize Test State Object 
            State = new TestStateInfo();

            // Init shared helpers/pages
            NavigationHelper = new NavigationHelper(Driver);
        
            SignInPage = new SignInPage(Driver);
            SignInSteps = new SignInSteps(Driver);

            Cleanup = new CleanupManager(Driver, State);


            // Start reporting
            CurrentTest = Extent.CreateTest(TestContext.CurrentContext.Test.Name);

            var categories = TestContext.CurrentContext.Test.Properties["Category"];

           

            // Only sign in if the test is NOT tagged as "SignIn"
            if (!categories.Contains("SignIn"))
            {
                               
                NavigationHelper.NavigateTo("");           // Go to home or reset
                NavigationHelper.GoToSignInPage();         // Navigate to Sign-In page

                var user = TestDataHelper.GetUserData("TestUser");
                SignInSteps.doSignIn(user.Email,user.Password); // Perform login
                               
            }

            // Pre-test cleanup based on categories
            Cleanup.RunPreTestCleanup(categories);

        }

      [TearDown]
        public void TestTeardown()
        {
            //Take Screenshot for failed test
            var result = TestContext.CurrentContext.Result;

            if (result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), $"Screenshot_{DateTime.Now.Ticks}.png");
                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                screenshot.SaveAsFile(screenshotPath);
                CurrentTest?.Fail(result.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
            }
            else
            {
                CurrentTest?.Pass("Test passed.");
            }


            // Post-test cleanup based on categories
            var categories = TestContext.CurrentContext.Test.Properties["Category"];
            Cleanup.RunPostTestCleanup(categories);

            Driver?.Quit();
        }


        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            Extent.Flush();
            Console.WriteLine("[ExtentReport] Report flushed at end of test run.");


        }
      

    }
}
