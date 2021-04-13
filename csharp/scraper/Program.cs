using System;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();

            Scraper scraper = new Scraper(config);
            var dataAgent = new RedisDataAgent();

            while (true)
            {
                scraper.Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
                scraper.WaitTillLoaded("/html/body/div/div/div[2]/div[1]/div[1]/div/form");

                scraper.Driver.FindElement(By.XPath("//*[@id=\"user-name\"]")).SendKeys("standard_user");
                scraper.Driver.FindElement(By.XPath("//*[@id=\"password\"]")).SendKeys("secret_sauce");
                scraper.Driver.FindElement(By.XPath("//*[@id=\"login-button\"]")).Click();

                scraper.WaitTillLoaded("//*[@id=\"add-to-cart-sauce-labs-backpack\"]");

                var items_data = new Dictionary<string, List<Dictionary<string, string>>> {
                    {"data", new List<Dictionary<string, string>>()}
                };

                var elements = scraper.Driver.FindElements(By.ClassName("inventory_item_description"));
                foreach (IWebElement element in elements)
                {
                    string title = element.FindElement(By.ClassName("inventory_item_name")).Text;
                    string description = element.FindElement(By.ClassName("inventory_item_desc")).Text;
                    string price = element.FindElement(By.ClassName("inventory_item_price")).Text;
                    items_data["data"].Add(
                        new Dictionary<string, string>{
                        {"title", title},
                        {"description", description},
                        {"price", price}
                        }
                    );
                }
                string items_data_string = JsonSerializer.Serialize(items_data);
                dataAgent.SetStringValue("items", items_data_string);
                Console.WriteLine($"{items_data_string}");
                System.Threading.Thread.Sleep(10000);
            }
        }
    }
}
