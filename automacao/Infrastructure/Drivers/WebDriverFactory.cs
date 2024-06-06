using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace automacao.Infrastructure.Drivers
{
    public class WebDriverFactory
    {
        private IWebDriver _driver;

        public IWebDriver CreateDriver()
        {
            if (_driver == null)
            {
                _driver = new ChromeDriver();
            }
            return _driver;
        }

        public void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver = null;
            }
        }
    }
}
