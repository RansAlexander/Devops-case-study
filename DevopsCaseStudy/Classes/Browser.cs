using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace DevopsCaseStudy {
    public class Browser {

        private string browserName;
        private bool remote;

        public Browser() {
            this.browserName = "Chrome";
            this.remote = false;
        }

        public Browser(string browserName, bool remote) {
            this.browserName = browserName;
            this.remote = false;
        }

        public bool getRemote() {
            return this.remote;
        }
        
        public string getBrowserName() {
            return this.browserName;
        }

        public void setRemote(bool remote) {
            this.remote = remote;
        }
        
        public void setBrowserName(string browserName) {
            this.browserName = browserName;
        }

        public void setBrowser(string browser) {
            this.browserName = browser;
        }

        public IWebDriver startBrowser() {
            String username = "alexanderrans";
            String accesskey = "5HkDl21lMTsd562eUA48eSJgbe2OZeH7CcVahQ0e7DOXgAQT2n";
            String gridURL = "@hub.lambdatest.com/wd/hub";
            IWebDriver driver = null;
            
            if (this.remote == true) {
                DesiredCapabilities capabilities = new DesiredCapabilities();
                capabilities.SetCapability("user", username);
                capabilities.SetCapability("accessKey", accesskey);
                capabilities.SetCapability("build", "[C#] Demo of Web Scraping in Selenium");
                capabilities.SetCapability("name", "[C#] Demo of Web Scraping in Selenium");
                capabilities.SetCapability("platform", "Windows 10");
                capabilities.SetCapability("browserName", "Chrome");
                capabilities.SetCapability("version", "96.0");
                driver = new RemoteWebDriver(new Uri("https://" + username + ":" + accesskey + gridURL), capabilities,
                    TimeSpan.FromSeconds(600));
                driver.Manage().Window.Maximize();
                return driver;
            }
            else {
                switch (this.browserName) {
                    case "Chrome":
                        var option = new ChromeOptions();
                        option.AddArgument("--disable-logging");
                        option.AddArgument("--log-level=3");
                        driver = new ChromeDriver(option);
                        break; 
                    case "Firefox":
                        driver = new FirefoxDriver();
                        break;
                    case "Edge":
                        driver = new EdgeDriver();
                        break;
                    case "Opera":
                        driver = new OperaDriver();
                        break;
                    case "Safari":
                        driver = new SafariDriver();
                        break;
                }
                return driver;
            }
        }
    }
}