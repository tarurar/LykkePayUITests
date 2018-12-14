using System;
using System.IO;
using System.Reflection;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace LykkePayUITests
{
    public class InvoiceTests
    {
        private readonly string _portalUrl;
        private readonly string _user;
        private readonly string _password;

        public InvoiceTests()
        {
            _portalUrl = Environment.GetEnvironmentVariable("LykkePayPortalUrl");
            _user = Environment.GetEnvironmentVariable("LykkePayUser");
            _password = Environment.GetEnvironmentVariable("LykkePayPassword");
        }

        [Theory]
        [InlineData("chrome")]
        public void GotoMainPage_ShouldOpen(string browser)
        {
            using (var driver = CreateWebDriver(browser))
            {
                driver.Navigate().GoToUrl(_portalUrl);

                var signIn = driver.FindElement(By.ClassName("header_login__title"));

                Assert.Equal("Sign in", signIn?.Text);
            }
        }

        [Theory]
        [InlineData("chrome")]
        public void SignInWithValidCredentials_ShouldOpenPaymentsPage(string browser)
        {
            using (var driver = CreateWebDriver(browser))
            {
                driver.Navigate().GoToUrl(_portalUrl);

                UiActions.SignIn(driver, _user, _password);

                var loggedInImage = driver.FindElement(By.ClassName("header_user__img"));

                Assert.NotNull(loggedInImage);
            }
        }

        [Theory]
        [InlineData("chrome")]
        public void CreateInvoice_ShouldAppearInList(string browser)
        {
            using (var driver = CreateWebDriver(browser))
            {
                driver.Navigate().GoToUrl(_portalUrl);

                UiActions.SignIn(driver, _user, _password);

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

                var invoiceNumber = $"auto_{Utils.RandomString(5)}";

                UiActions.CreateInvoice(driver, invoiceNumber, "auto", "auto@test.com", 1);

                var invoiceNumberElement = driver.FindElement(
                    By.XPath($"//div[contains(@class, 'asset_link__desc') and text() = '{invoiceNumber}']"));

                Assert.NotNull(invoiceNumberElement);
            }
        }

        private static IWebDriver CreateWebDriver(string browserName)
        {
            switch (browserName.ToLowerInvariant())
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("headless");
                    return new ChromeDriver(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), chromeOptions);

                case "firefox":
                    var ffOptions = new FirefoxOptions();
                    ffOptions.AddArgument("headless");
                    return new FirefoxDriver(ffOptions);

                default:
                    throw new NotSupportedException($"The browser '{browserName}' is not supported.");
            }
        }
    }
}
