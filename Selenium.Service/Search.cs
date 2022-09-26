using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.Data.Model;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Selenium.Service
{
    public class Search
    {
        public static List<SteamGameModel> GetAllSteamGame()
        {
            List<SteamGameModel> list = new List<SteamGameModel>();

            for (int i = 0; i < int.MaxValue; i++)
            {
                SteamGameModel model = new SteamGameModel();

                ChromeOptions options = new ChromeOptions();
                var chromeDriverService = ChromeDriverService.CreateDefaultService(Environment.GetEnvironmentVariable("ChromeDriverPath"));
                chromeDriverService.HideCommandPromptWindow = true;
                chromeDriverService.SuppressInitialDiagnosticInformation = true;
                options.AddArgument("headless");
                options.AddArgument("--silent");
                options.AddArgument("log-level=3");
                IWebDriver chromeDriver = new ChromeDriver(chromeDriverService, options);

                try
                {
                    model.Id = i;
                    chromeDriver.Url = "https://store.steampowered.com/points/shop/app/" + i;

                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    model.Name = chromeDriver.FindElement(By.ClassName("page_PageTitle_2g7YP")).Text;
                    model.IsExist = !string.IsNullOrEmpty(model.Name);
                }
                catch (Exception ex)
                {
                    model.Name = ex.Message;
                    model.IsExist = false;
                }
                finally
                {
                    list.Add(model);
                    chromeDriver.Quit();
                }
            }
            return list;
        }

        public static List<SteamGameModel> GetAllSteamGameAsync()
        {
            List<SteamGameModel> list = new List<SteamGameModel>();

            Parallel.For(0, int.MaxValue, new ParallelOptions() { MaxDegreeOfParallelism = 25 }, i =>
            {
                SteamGameModel model = new SteamGameModel();

                ChromeOptions options = new ChromeOptions();
                var chromeDriverService = ChromeDriverService.CreateDefaultService(Environment.GetEnvironmentVariable("ChromeDriverPath"));
                chromeDriverService.HideCommandPromptWindow = true;
                chromeDriverService.SuppressInitialDiagnosticInformation = true;
                options.AddArgument("headless");
                options.AddArgument("--silent");
                options.AddArgument("log-level=3");
                IWebDriver chromeDriver = new ChromeDriver(chromeDriverService, options);

                try
                {
                    Console.WriteLine(i);
                    model.Id = i;
                    chromeDriver.Url = "https://store.steampowered.com/points/shop/app/" + i;

                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    model.Name = chromeDriver.FindElement(By.ClassName("page_PageTitle_2g7YP")).Text;
                    model.IsExist = !string.IsNullOrEmpty(model.Name);
                }
                catch (Exception ex)
                {
                    model.Name = ex.Message;
                    model.IsExist = false;
                }
                finally
                {
                    list.Add(model);
                    chromeDriver.Quit();
                }
            });
            return list.OrderBy(x => x.Id).ToList();
        }

        public static void SeacrhOnHepsiBurada(string searchKey)
        {
            using (IWebDriver chromeDriver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeDriverPath")))
            {
                chromeDriver.Url = "https://www.hepsiburada.com/";
                chromeDriver.FindElement(By.ClassName("desktopOldAutosuggestTheme-UyU36RyhCTcuRs_sXL9b")).SendKeys(searchKey);
                chromeDriver.FindElement(By.ClassName("SearchBoxOld-cHxjyU99nxdIaAbGyX7F")).Click();
                chromeDriver.Quit();
            }
        }
    }
}