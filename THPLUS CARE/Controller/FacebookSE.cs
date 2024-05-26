using OpenQA.Selenium.DevTools.V123.FedCm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Data.SqlTypes;
using System.Threading;

namespace THPLUS_CARE.Controller
{
    internal class FacebookSE
    {
        public static void  Login(string uid,string pass)
        {
            string profilePath = @"C:\THCARE\Profile\"+ uid;

            // Create ChromeOptions

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
         //   chromeDriverService.SuppressInitialDiagnosticInformation = true;

            // Set the path to the new profile
            // Create a new instance of the ChromeDriver
            var chromeOptions = new ChromeOptions();

  

            // Define proxy settings
            string proxyAddress = "183.88.212.252";
            int proxyPort = 14238;
            string proxyUsername = "nz5QFs";
            string proxyPassword = "eqw68a";



            
            // Set proxy settings with authentication
            chromeOptions.AddArgument($"--proxy-server=http://{proxyAddress}:{proxyPort}");


            chromeOptions.AddArgument("user-data-dir=" + profilePath);

            chromeOptions.AddArgument("--no-sandbox"); // Maximize the browser window
            chromeOptions.AddArgument("--log-level=3"); // Maximize the browser window
            chromeOptions.AddArgument("--disable-web-security"); // Maximize the browser window
            chromeOptions.AddArgument("--silent"); // Maximize the browser window
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--disable-crash-reporter");
            chromeOptions.AddArgument("--disable-extensions");
            chromeOptions.AddArgument("--disable-in-process-stack-traces");
            chromeOptions.AddArgument("--disable-logging");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            
            //chromeOptions.AddArgument("--headless"); // Maximize the browser window

            chromeOptions.AddArgument("start-maximized"); // Maximize the browser window

            IWebDriver driver = new ChromeDriver(chromeDriverService,chromeOptions);
            INetwork networkInterceptor = driver.Manage().Network;


            NetworkAuthenticationHandler handler = new NetworkAuthenticationHandler()
            {
                UriMatcher = (d) => true,
                Credentials = new PasswordCredentials(proxyUsername, proxyPassword)
            };


            networkInterceptor.AddAuthenticationHandler(handler);
            networkInterceptor.StartMonitoring();
            networkInterceptor.StopMonitoring();


            // Navigate to a webpage
            driver.Navigate().GoToUrl("https://www.facebook.com/");

            if (driver.Title.Contains(" log in or sign up"))
            {

                // Find the input field by its ID (replace "inputFieldId" with the actual ID of your input field)
                IWebElement email = driver.FindElement(By.Name("email"));

                // Clear the input field (optional)
                email.Clear();

                // Set the text in the input field
                email.SendKeys(uid);


                IWebElement password = driver.FindElement(By.Id("pass"));

                // Clear the input field (optional)
                password.Clear();

                // Set the text in the input field
                password.SendKeys(pass);


                IWebElement button = driver.FindElement(By.Name("login"));

                // Click the button
                button.Click();
            }
            else
            {
                Console.WriteLine("LOGIN ALREAY");
            }
            // Get and print the page title

            // Wait until the page is fully loaded
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(webDriver => ((IJavaScriptExecutor)webDriver).ExecuteScript("return document.readyState").Equals("complete"));

            // Close the browser
            //  Thread.Sleep(10000);
            driver.Quit();
        }
    }
}
