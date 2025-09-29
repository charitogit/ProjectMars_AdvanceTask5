using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Utilities
{
    public class Wait
    {

        public static void WaitToBeVisible(IWebDriver driver, string locType, string locValue, int seconds)

        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, seconds));

            if (locType == "XPath")

            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(locValue)));
            }

            if (locType == "Id")

            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id(locValue)));
            }


        }


        public static void WaitToBeClickable(IWebDriver driver, string locType, string locValue, int timeoutSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));

            By bySelector = locType switch
            {
                "XPath" => By.XPath(locValue),
                "Id" => By.Id(locValue),
                _ => throw new ArgumentException($"Unsupported locator type: {locType}")
            };

            wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(bySelector);
                    if (element != null && element.Displayed && element.Enabled)
                        return element;  // element is clickable, return it

                    return null; // keep waiting
                }
                catch (StaleElementReferenceException)
                {
                    // Element became stale, retry finding
                    return null;
                }
                catch (NoSuchElementException)
                {
                    // Element not present yet, keep waiting
                    return null;
                }
            });
        }


        public static void WaitToExist(IWebDriver driver, string locType, string locValue, int seconds)

        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, seconds));

            if (locType == "XPath")

            {
                wait.Until(ExpectedConditions.ElementExists(By.XPath(locValue)));
            }

            if (locType == "Id")

            {
                wait.Until(ExpectedConditions.ElementExists(By.Id(locValue)));
            }


        }

      
    }
}
