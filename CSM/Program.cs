using System;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CSM
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHelper wh = new WebHelper();
            wh.generateMail();
            //wh.Quit();
            Console.ReadKey();
        }
    }
    public class WebHelper
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;
        private Random rnd;
        private double waitPeriod;

        public WebHelper()
        {
            driver = new ChromeDriver();
            baseURL = "https://scryptmail.com/";
            verificationErrors = new StringBuilder();
            rnd = new Random();
            waitPeriod = 30.0;
        }

        public void Quit()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        public void generateMail()
        {
            string login = "TRVE.MAN." + (System.DateTime.Now.ToString()).GetHashCode();
            string pass = (login.GetHashCode()).ToString();
            try
            {
                do_goToStartPage();
                wait(2, 4);
                do_userEmailClickBtn();
                wait(2, 4);
                do_typePassword(pass);
                wait(2, 4);
                do_typePasswordRepeat(pass);
                wait(2, 4);
                do_typeUserEmail(login);
                wait(3, 5);
                do_checkEmail();
                wait(2, 4);
                do_createEmailBtn();
                wait(2, 4);
                log(String.Format(@"/////////////////////////"));
                log(String.Format("{0}\t{1}", login, pass));
                log(String.Format(@"\\\\\\\\\\\\\\\\\\\\\\\\\"));
                //do_clickCloseBtn(login, pass);
                wait(310, 360);
                generateMail();
            }
            catch (Exception ex)
            {
                log(ex.Message);
                wait(3, 5);
                generateMail();
            }
            
        }
        public void do_goToStartPage()
        {
            driver.Navigate().GoToUrl(baseURL + "/");
            //log("[v] Переход на стартовую стираничку");
        }
        public void do_userEmailClickBtn()
        {
            By by = By.CssSelector("a.btn.btn-primary");
            //By by = By.XPath("(//a[contains(text(),'Sign Up For Free')])[2]");
            if (IsElementPresent(by))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(waitPeriod);
                driver.FindElement(by).Click();
                //log("[v] Нажата стартовая кнопка");
            }
            else
            {
                log("[x] Нажата стартовая кнопка неудачно");
            }
        }
        public void do_typeUserEmail(string login)
        {
            By by = By.Id("userEmail");
            if (IsElementPresent(by))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(waitPeriod);
                foreach (char ch in login)
                {
                    driver.FindElement(by).SendKeys(ch.ToString());
                    Thread.Sleep(rnd.Next(90, 140));
                }
                //log("[v] Логин введен");
            }
            else
            {
                log("[x] Логин введен неудачно");
            }
        }
        public void do_typePassword(string pass)
        {
            By by = By.Id("userPassword");
            if (IsElementPresent(by))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(waitPeriod);
                foreach (char ch in pass)
                {
                    driver.FindElement(by).SendKeys(ch.ToString());
                    Thread.Sleep(rnd.Next(90, 140));
                }
                //log("[v] Пароль введен");
            }
            else
            {
                log("[x] Пароль введен неудачно");
            }
        }
        public void do_typePasswordRepeat(string pass)
        {
            By by = By.Id("userPasswordRepeat");
            if (IsElementPresent(by))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(waitPeriod);
                foreach (char ch in pass)
                {
                    driver.FindElement(by).SendKeys(ch.ToString());
                    Thread.Sleep(rnd.Next(90, 140));
                }
                //log("[v] Пароль2 введен");
            }
            else
            {
                log("[x] Пароль2 введен неудачно");
            }
        }
        public void do_checkEmail()
        {
            By by = By.CssSelector(".control-label.pull-left");
            if (IsElementPresent(by))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(waitPeriod);
                if (driver.FindElement(by).Text == "email exists")
                {
                    // Надо рестарт вставить
                    log("[x] Логин уже существует");
                    generateMail();
                }
                else
                {
                    //log("[v] Логин не существует");
                }
            }
            else
            {
                //log("[v] Логин не существует");
            }
        }
        public void do_createEmailBtn()
        {
            By by = By.CssSelector("#createUser > div.form-group > #reguser");
            if (IsElementPresent(by))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(waitPeriod);
                driver.FindElement(by).Click();
                //log("[v] Нажата кнопка создания почты");
            }
            else
            {
                log("[x] Кнопка создания почты не нажата");
            }
        }
        public void do_clickCloseBtn(string login, string pass)
        {
            log(String.Format("{0}\t{1}", login, pass));
            By by = By.CssSelector("button.btn.btn-default");
            if (IsElementPresent(by))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(waitPeriod);
                driver.FindElement(by).Click();
                //log("[v] Нажата кнопка выход");
            }
            else
            {
                log("[x] Не нажата кнопка выход");
            }
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }

        private void log(string str)
        {
            Console.WriteLine(str);
        }
        private void wait(int min, int max)
        {
            Thread.Sleep(rnd.Next(900, 1100) * rnd.Next(min, max));
        }
    }//class helper
}
