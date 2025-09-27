ProjectMars Advance Task - NUnit
================================
NUnit-based Test Automation Framework for Project Mars web application using:

- NUnit (.NET Test Framework)
- Selenium WebDriver
- ExtentReports for rich test reports
- JSON-based test data (for profile and skill features)
- Page Object Model (POM)
- Clean Dependency Injection (DI)
- .NET 8

--------------------------------
Project Structure
--------------------------------

ProjectMars_AdvanceTask_NUnit/
├── Config/        # Configuration classes (settings, environments)
├── Helpers/       # Utility classes (Waits, ClickHelper, navigation helpers,TestDataHelpers)
├── Hooks/         # NUnit setup/teardown hooks -TestBase(Driver,SignIn reporting,Screenshots) and  CleanupManager(data cleanup)
├── Models/        # Data models for JSON deserialization
├── Pages/         # Page Object Model classes
├── Steps/         # Step classes representing user actions (load JSON data(via TestDataHelper) + call Pages)
├── TestData/      # JSON files for test inputs (Skills, Languages,ProfileDetail,ShareSkill,SearchSkill,Notification etc.)
├── Tests/         # NUnit test classes
├── TestStates/    # State-sharing classes between steps
├── Utilities/     # Common utilities (Wait)
├── settings.json  # Config for browser, URL, timeout, report path
└── README.txt     # Project documentation

--------------------------------
Execution Workflow (ASCII Diagram)
--------------------------------

+---------+       +---------+       +---------+       +---------+       +-----------+
|  Hooks  | --->  |  Tests  | --->  |  Steps  | --->  |  Pages  | --->  | Assertions|
+---------+       +---------+       +---------+       +---------+       +-----------+

Workflow Explanation:

1. Hooks
   - OneTimeSetup: Initializes ExtentReports
   - BeforeTest: Sets up WebDriver and TestStateObject
   - AfterTest: Tears down WebDriver
   - OneTimeTearDown: Flushes ExtentReports

2. Tests
   - NUnit test classes referencing Steps

3. Steps
   - Load JSON data (Models)
   - Map JSON data into strongly typed objects
   - Call Page Object methods
   - Manage TestState

4. Pages
   - Encapsulate locators and UI interactions
   - Keep tests clean and maintainable

5. Assertions
   - Verify UI and data against expected results
  

--------------------------------
JSON Data Structure (Sample)
--------------------------------

Skill test data is stored in TestData/skill.json:

{
  "skillName": "Communication",
  "level": "Intermediate",
  "expectedMessage": "Communication has been added to your skills"
}

This maps to the following model class:

  public class Skill
  {
      public string SkillName { get; set; }
      public string Level { get; set; }
      public string ExpectedMessage { get; set; }

     
  }


--------------------------------
Getting Started
--------------------------------

Prerequisites:
- .NET 8 SDK
- Google Chrome (or supported browser)
- NuGet dependencies restored automatically

Setup & Run Tests:
dotnet restore
dotnet test

After execution, open the generated report:
C:\MVP Internship\AdvanceTask\ProjectMars_AdvanceTask_NUnit\bin\TestReport.html

--------------------------------
Features Covered
--------------------------------

| Module       | Coverage                                                                 |
| ------------ | ------------------------------------------------------------------------ |
| Sign In      | Valid/Invalid login (Hook-based login for dependent tests)               |
| Language     | Add, Edit, Delete, Boundary checks (e.g., empty/duplicate entries)       |
| Skills       | Add, Edit, Delete, Duplicate checks, Invalid field scenarios             |
| Share Skill  | Create listings with validation for mandatory/optional fields            |
| Search Skill | Search by category, subcategory, and keywords                            |
| Notification | Mark As Read, Delete                            |

--------------------------------
Hooks  
--------------------------------

| Hook     	      | Purpose                                                                                       |
| ------------------- | --------------------------------------------------------------------------------------------- |
| Driver              | Initializes WebDriver before each test and quits after test                                     |
| TestStateInfo       | Stores data created or modified during tests (e.g., Language, Skill, ShareSkill entries)       |
|                     | Steps add test records to TestStateInfo; CleanupHook uses it to delete only test-added data    |
| Report              | Creates Extent Report in OneTimeSetup, flushes after all tests                                   |
| Cleanup             | Performs pre- and post-test cleanup of test data: Language, Skill, ShareSkill, Notifications   |


--------------------------------
Extent Reports
--------------------------------

- Generated to: ProjectMars_AdvanceTask_NUnit\bin\TestReport.html
- Lifecycle:
  - Created ONCE at framework startup (`[OneTimeSetUp]`)
  - Logs test steps and screenshots during execution
  - Finalized ONCE after all tests finish (`[OneTimeTearDown]`)
- Includes:
  - Pass/Fail step logging
  - Screenshots on failure (Reports/Screenshots)
  - Test duration, environment details, and metadata

Sample System Info Logged:
| Name        | Value                                            |
| ----------- | ------------------------------------------------ |
| Environment | http://localhost:5000/                           |
| Browser     | Chrome                                           |

--------------------------------
Configuration File: settings.json
--------------------------------

{
  "Browser": {
    "Type": "Chrome",
    "Headless": false,
    "TimeoutSeconds": 5
  },
  "Report": {
    "Path": "TestReport.html",
    "Title": "Test Automation Report"
  },
  "Environment": {
    "BaseUrl": "http://localhost:5003/"
  }
}


--------------------------------
Credentials for Automation Testing
--------------------------------

Automation test credentials are stored in `TestData/TestUser.json`:

{
  "Email": "charie_artz@yahoo.com",
  "Password": "P@ssw0rd",
  "Greeting": "Hi Charito"
}

Usage:
- SignIn via Hooks (TestBase)  
- SignIn via Steps (SignInStep)  

 Both approach read credentials from the JSON file using the Models helper via TestDataHelper:

  var user = TestDataHelper.GetUserData("TestUser");
  SignInSteps.doSignIn(user.Email,user.Password); // Perform login

- You can also use the "Greeting" field for personalized test verifications.
