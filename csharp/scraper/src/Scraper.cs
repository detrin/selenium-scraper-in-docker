using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Microsoft.Extensions.Configuration;

namespace scraper
{
    public class Scraper
    {
        protected string browserDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        protected Dictionary<string, string> xpaths = new Dictionary<string, string>();
        protected float waitAfterAction = 0.2f;

        public string Url { get; set; }
        public string BaseUrl { get; set; }
        public bool Visibility { get; set; }
        public string UserAgent { get; set; }
        public string CollectionName { get; set; }
        public IWebDriver Driver { get; set; }

        public Scraper(IConfigurationRoot configurationRoot)
        {
            string userAgent = configurationRoot.GetSection("UserAgent").Get<string>();

            ChromeOptions chromeOptions = GetOptions(userAgent);
            Driver = new ChromeDriver(chromeOptions);
        }

        public ChromeOptions GetOptions(string userAgent)
        {
            ChromeOptions options = new ChromeOptions();
            // options.AddArguments("--start-maximized");
            options.AddArguments($"user-agent={UserAgent}");
            options.AddArguments("--headless");
            options.AddArguments("--no-sandbox");
            options.AddArguments("--disable-dev-shm-usage");
            return options;
        }

        public IWebElement WaitTillLoaded(string xpath)
        {
            var wait = new WebDriverWait(Driver, new TimeSpan(0, 0, 1, 0));
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(xpath)));
            return element;
        }

        public IWebElement WaitTillLoaded(string xpath, int loadSeconds)
        {
            var wait = new WebDriverWait(Driver, new TimeSpan(0, 0, 0, loadSeconds));
            IWebElement element = wait.Until(
              SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(xpath))
            );
            return element;
        }

        public void ClickElement(string xpath)
        {
            Driver.FindElement(By.XPath(xpath)).Click();
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(waitAfterAction));
        }

        public void FillText(string xpath, string text)
        {
            Driver.FindElement(By.XPath(xpath)).SendKeys(text);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(waitAfterAction));
        }

        public virtual bool IsElementPresent(By by)
        {
            try
            {
                Driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public virtual bool IsElementPresent(string xpath)
        {
            try
            {
                Driver.FindElement(By.XPath(xpath));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public Func<IWebDriver, bool> IsAnyOfElementsPresent(By by1, By by2)
        {
            return (driver) =>
                {
                    try
                    {
                        IWebElement e1 = driver.FindElement(by1);
                        return e1.Displayed;
                    }
                    catch (Exception)
                    {
                        try
                        {
                            IWebElement e2 = driver.FindElement(by2);
                            return e2.Displayed;
                        }
                        catch (Exception)
                        {
                            // If element is null, stale or if it cannot be located
                            return false;
                        }
                    }
                };
        }

    }
}
