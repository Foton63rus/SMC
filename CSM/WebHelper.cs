using System;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace CSM
{
    class WebHelper
    {
        private IWebDriver driver;
        private string baseURL;
        private Random rnd;
        private double waitPeriod;
        private string logs;
        private string pathLogs;
        private string pathLP;
        public WebHelper()
        {
            OpenQA.Selenium.Chrome.ChromeDriverService service = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService();
            OpenQA.Selenium.Chrome.ChromeOptions chromeOptions = new OpenQA.Selenium.Chrome.ChromeOptions();
            driver = new ChromeDriver(service, chromeOptions, TimeSpan.FromSeconds(180));
            baseURL = "https://scryptmail.com/";
            rnd = new Random();
            waitPeriod = 30.0;
            logs = "";
            pathLogs = System.IO.Directory.GetCurrentDirectory()+@"\logs.txt";
            pathLP = System.IO.Directory.GetCurrentDirectory() + @"\lp.txt";
            checkLogsFile();
            checkLPFile();
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
        /// <summary>
        /// алгоритм регистрации новой почты
        /// </summary>
        public void generateMail()
        {
            int i = Math.Abs( (System.DateTime.Now.ToString()).GetHashCode() );
            string login = "TRVEMAN" + i.ToString();
            i = Math.Abs(login.GetHashCode());
            string pass = "Sm"+i.ToString();
            try
            {
                log("new generating " + DateTime.Now.ToString());
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
                log(String.Format("added {0}\t{1}", login + "@scryptmail.com", pass));
                Console.WriteLine("{0}\t{1}", login + "@scryptmail.com", pass);
                saveLogPass(login, pass);
                saveLogs();
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
        /// <summary>
        /// переход на стартовую страничку
        /// </summary>
        public void do_goToStartPage()
        {
            driver.Navigate().GoToUrl(baseURL + "/");
            log("[v] Переход на стартовую стираничку");
        }
        /// <summary>
        /// клацаем кнопку новой регистрации почты
        /// </summary>
        public void do_userEmailClickBtn()
        {
            By by = By.CssSelector("a.btn.btn-primary");
            if (IsElementPresent(by))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(waitPeriod);
                driver.FindElement(by).Click();
                log("[v] Нажата стартовая кнопка");
            }
            else
            {
                log("[x] Нажата стартовая кнопка неудачно");
            }
        }
        /// <summary>
        /// вводим логин
        /// </summary>
        /// <param name="login"></param>
        public void do_typeUserEmail(string login)
        {
            By by = By.Id("userEmail");
            if (IsElementPresent(by))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(waitPeriod);
                type(by, login);
            }
            else
            {
                log("[x] Логин введен неудачно");
            }
        }
        /// <summary>
        /// Вводим пароль
        /// </summary>
        /// <param name="pass"></param>
        public void do_typePassword(string pass)
        {
            By by = By.Id("userPassword");
            if (IsElementPresent(by))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(waitPeriod);
                type(by, pass);
            }
            else
            {
                log("[x] Пароль введен неудачно");
            }
        }
        /// <summary>
        /// Вводим пароль повторно
        /// </summary>
        /// <param name="pass"></param>
        public void do_typePasswordRepeat(string pass)
        {
            By by = By.Id("userPasswordRepeat");
            if (IsElementPresent(by))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(waitPeriod);
                type(by, pass);
            }
            else
            {
                log("[x] Пароль2 введен неудачно");
            }
        }
        /// <summary>
        /// Проверочка почты на существования с таким логином
        /// </summary>
        public void do_checkEmail()
        {
            By by = By.CssSelector(".control-label.pull-left");
            if (IsElementPresent(by))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(waitPeriod);
                if (driver.FindElement(by).Text == "email exists")
                {
                    log("[x] Логин уже существует");
                    generateMail();
                }
                else
                {
                    log("[v] Логин не существует");
                }
            }
            else
            {
                log("[v] Логин не существует");
            }
        }
        /// <summary>
        /// нажать кнопку создания почты по введенным данным
        /// </summary>
        public void do_createEmailBtn()
        {
            By by = By.CssSelector("#createUser > div.form-group > #reguser");
            if (IsElementPresent(by))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(waitPeriod);
                driver.FindElement(by).Click();
                log("[v] Нажата кнопка создания почты");
            }
            else
            {
                log("[x] Кнопка создания почты не нажата");
            }
        }
        /// <summary>
        /// есть ли элемент с указанным параметром поиска
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
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
        /// <summary>
        /// сохранить логи в файлик
        /// </summary>
        private void saveLogs()
        {
            try
            {
                File.AppendAllText(pathLogs, logs, Encoding.UTF8);
                logs = "";
            }
            catch
            {
                //skip nulling logs
            }
        }
        /// <summary>
        /// Сохранить логин / пароль в файлик
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        private void saveLogPass(string login, string pass)
        {
            try
            {
                File.AppendAllText(pathLP, String.Format("{0}\t{1}" + Environment.NewLine, login + "@scryptmail.com", pass));
            }
            catch
            {
                log("Не получилось записать логи");
            }
        }
        private void log(string str)
        {
            logs += (str + Environment.NewLine);
        }
        private void wait(int min, int max)
        {
            Thread.Sleep(rnd.Next(900, 1100) * rnd.Next(min, max));
        }
        /// <summary>
        /// печатает текст в нужный элемент
        /// </summary>
        /// <param name="by"></param>
        /// <param name="text"></param>
        private void type(By by, string text)
        {
            foreach (char ch in text)
            {
                driver.FindElement(by).SendKeys(ch.ToString());
                Thread.Sleep(rnd.Next(90, 140));
            }
        }
        private void checkLogsFile()
        {
            if (File.Exists(pathLogs))
            {
                //File.AppendAllText(pathLogs, "***" + DateTime.Now.ToString() + "***" + Environment.NewLine);
                File.WriteAllText(pathLogs, "");
            }
            else
            {
                File.Create(pathLogs);
            }
        }
        private void checkLPFile()
        {
            if (!File.Exists(pathLP))
                File.Create(pathLP);
        }
    }
}
