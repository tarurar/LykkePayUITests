using System.Globalization;
using OpenQA.Selenium;

namespace LykkePayUITests
{
    public static class UiActions
    {
        /// <summary>
        /// Signs in user.
        /// Assumes main portal page is opened and user is not logged in.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public static void SignIn(IWebDriver driver, string userName, string password)
        {
            driver.FindElement(By.LinkText("Sign in")).Click();
            driver.FindElement(By.Id("email")).SendKeys(userName);
            driver.FindElement(By.Id("password")).SendKeys(password);
            driver.FindElement(By.ClassName("lp-signin-btn")).Submit();
        }

        /// <summary>
        /// Creates invoice.
        /// Assumes payments page is opened and user is logged in.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="invoiceNumber"></param>
        /// <param name="client"></param>
        /// <param name="email"></param>
        /// <param name="amount"></param>
        public static void CreateInvoice(IWebDriver driver, string invoiceNumber, string client, string email, decimal amount)
        {
            driver.FindElement(By.ClassName("payments_header__btn")).Click();
            driver.FindElement(By.Name("number")).SendKeys(invoiceNumber);
            driver.FindElement(By.Name("client")).SendKeys(client);
            driver.FindElement(By.Name("email")).SendKeys(email);
            driver.FindElement(By.Name("amount")).SendKeys(amount.ToString(CultureInfo.InvariantCulture));
            driver.FindElement(By.Name("note")).SendKeys("Generated automatically");
            
            // workaround to click on some buttons
            var generateButton = driver.FindElement(By.XPath("//button[.='Generate']"));
            generateButton.SendKeys(Keys.Enter);
        }
    }
}