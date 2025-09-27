using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Helpers
{
    public class ClickHelper
    {
        // Scrolls element into view and performs a click
        public static void ScrollIntoViewAndClick(IWebDriver driver, IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
        arguments[0].scrollIntoView({ behavior: 'auto', block: 'center' });
        arguments[0].click();", element);
        }

        // Fires a custom JS event on an element (like 'click', 'change', 'mouseover')
        public static void FireJsEvent(IWebDriver driver, IWebElement element, string eventName)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
            var evt = new Event(arguments[1], { bubbles: true });
            arguments[0].dispatchEvent(evt);", element, eventName);
        }

    }
}
