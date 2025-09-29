using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Helpers
{
    public class AssertionHelper
    {
        public static void AssertToastMessage(string actualMessage, string expectedMessage)
        {
            if (actualMessage == null || expectedMessage == null)
            {
                throw new ArgumentNullException("Actual or Expected message cannot be null");
            }

            Assert.That(actualMessage, Is.EqualTo(expectedMessage), $"Expected message '{expectedMessage}' does not match actual message '{actualMessage}'");
        }

        public static void AssertRecordPresent(bool isPresent, string recordName)
        {
            if (string.IsNullOrWhiteSpace(recordName))
                recordName = "Record";

            Assert.That(isPresent, Is.True, $"{recordName} should be present.");
        }
        public static void AssertRecordNotPresent(bool isPresent, string recordName)
        {
            if (string.IsNullOrWhiteSpace(recordName))
                recordName = "Record";

            Assert.That(isPresent, Is.False, $"{recordName} should not be present.");
        }

        public static void AssertUrlContains(IWebDriver driver, string expectedPartialUrl, string pageDescription)
        {
            if (string.IsNullOrWhiteSpace(pageDescription))
                pageDescription = "page";

            Assert.That(driver.Url, Does.Contain(expectedPartialUrl),
                $"Did not navigate to {pageDescription} where action was expected.");
        }


        public static void AssertCountEqual(int actualCount, int expectedCount, string itemDescription)
        {
           
            Assert.That(actualCount, Is.EqualTo(expectedCount),
                $"Actual count of {itemDescription} ({actualCount}) does not match expected count ({expectedCount}).");
        }


    }
}
