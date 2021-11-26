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
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization.Metadata;

namespace ConsoleApp1
{
    public class Browser
    {
        private string url, browser, text;
        private bool remote;
        
        public Browser()
        {
            this.url = "https://www.youtube.com/c/LambdaTest/videos;";
            this.browser = "Chrome";
            this.remote = false;
        }
        public Browser(string url)
        {
            this.url = url;
            this.browser = "Chrome";
            this.remote = false;
        }

        public Browser(string url, string browser, bool remote)
        {
            this.url = url;
            this.browser = browser;
            this.remote = remote;
        }

        public IWebDriver startBrowser()
        {
            String test_url_1 = url;
            String username = "alexanderrans";
            String accesskey = "5HkDl21lMTsd562eUA48eSJgbe2OZeH7CcVahQ0e7DOXgAQT2n";
            String gridURL = "@hub.lambdatest.com/wd/hub";
            IWebDriver driver = null;

            if (remote)
            {
                DesiredCapabilities capabilities = new DesiredCapabilities();
                capabilities.SetCapability("user", username);
                capabilities.SetCapability("accessKey", accesskey);
                capabilities.SetCapability("build", "[C#] Demo of Web Scraping in Selenium");
                capabilities.SetCapability("name", "[C#] Demo of Web Scraping in Selenium");
                capabilities.SetCapability("platform", "Windows 10");
                capabilities.SetCapability("browserName", browser);
                capabilities.SetCapability("version", "96.0");
                driver = new RemoteWebDriver(new Uri("https://" + username + ":" + accesskey + gridURL), capabilities,
                    TimeSpan.FromSeconds(600));
                driver.Manage().Window.Maximize();
                return driver;

            }
            else
            {
                switch (browser)
                {
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

        public void getAllVideos()
        {
            text = "";
            IWebDriver driver = startBrowser();
            Int32 vcount = 1;
            driver.Url = url;
            /* Explicit Wait to ensure that the page is loaded completely by reading the DOM state */
            var timeout = 10000; /* Maximum wait time of 10 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            
            Thread.Sleep(1000);
            // Youtube asks for you to accept cookies or whatever bullshit so this is to check if it does and accept it
            String pageTitle=driver.Title;
            // Console.WriteLine(pageTitle);
            // google asks to agree with coockies or some dumb shit which fucks with selenium
            if (pageTitle == "Before you continue to YouTube")
            {
                driver.FindElement(By.XPath("//*[contains(text(), 'I agree')]")).Click();
                Thread.Sleep(1000);
            }
            // pageTitle=driver.Title;
            // Console.WriteLine(pageTitle);
            /* Once the page has loaded, scroll to the end of the page to load all the videos */
            /* Scroll to the end of the page to load all the videos in the channel */
            /* Reference - https://stackoverflow.com/a/51702698/126105 */
            /* Get scroll height */
            Int64 last_height = (Int64)(((IJavaScriptExecutor)driver).ExecuteScript("return document.documentElement.scrollHeight"));
            while (true)
            {
                pageTitle=driver.Title;
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.documentElement.scrollHeight);");
                /* Wait to load page */
                Thread.Sleep(2000);
                /* Calculate new scroll height and compare with last scroll height */
                Int64 new_height = (Int64)((IJavaScriptExecutor)driver).ExecuteScript("return document.documentElement.scrollHeight");
                if (new_height == last_height)
                    /* If heights are the same it will exit the function */
                    break;
                last_height = new_height;
            }
 
            By elem_video_link = By.CssSelector("ytd-grid-video-renderer.style-scope");
            ReadOnlyCollection<IWebElement> videos = driver.FindElements(elem_video_link);
            String title=driver.Title;
            Console.WriteLine("Page title is : " + title);
            Console.WriteLine("Total number of videos in " + url + " are " + videos.Count);

            /* Go through the Videos List and scrap the same to get the attributes of the videos in the channel */
            
            foreach (IWebElement video in videos)
            {
                string str_title, str_views, str_rel, csvtext;
                IWebElement elem_video_title = video.FindElement(By.CssSelector("#video-title"));
                str_title = elem_video_title.Text;
 
                IWebElement elem_video_views = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[1]"));
                str_views = elem_video_views.Text;
 
                IWebElement elem_video_reldate = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[2]"));
                str_rel = elem_video_reldate.Text;
 
                Console.WriteLine("******* Video " + vcount + " *******");
                Console.WriteLine("Video Title: " + str_title);
                Console.WriteLine("Video Views: " + str_views);
                Console.WriteLine("Video Release Date: " + str_rel);
                Console.WriteLine("\n");
                csvtext = $"{vcount};{str_title};{str_views}";
                generateCSV(csvtext);
                vcount++;
            }
            Console.WriteLine("Scraping Data from LambdaTest YouTube channel Passed");
            driver.Close();
        }

        public void getRecenVideos()
        {
            text = "";
            IWebDriver driver = startBrowser();
            Int32 vcount = 0;
            driver.Url = url;
            /* Explicit Wait to ensure that the page is loaded completely by reading the DOM state */
            var timeout = 10000; /* Maximum wait time of 10 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            
            Thread.Sleep(1000);
            // Youtube asks for you to accept cookies or whatever bullshit so this is to check if it does and accept it
            // google asks to agree with coockies or some dumb shit which fucks with selenium
            driver.FindElement(By.XPath("//*[contains(text(), 'I Agree')]")).Click();
            Thread.Sleep(1000);
            
            ReadOnlyCollection<IWebElement> videos = driver.FindElements(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[2]/ytd-item-section-renderer/div[3]/ytd-video-renderer[position()<=5]"));
            foreach (IWebElement video in videos)
            {
                string str_title, str_views, str_author;
                IWebElement elem_video_title = video.FindElement(By.CssSelector("#video-title"));
                str_title = elem_video_title.Text;
                
                IWebElement elem_video_views = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[1]"));
                str_views = elem_video_views.Text;
                string[] str_views_split = str_views.Split(" ");
                
                // IWebElement elem_video_author = video.FindElement(By.CssSelector("#channel-name a"));
                // str_author = elem_video_author.Text;
                IWebElement elem_video_author = video.FindElement(By.CssSelector("#channel-name a"));
                str_author = elem_video_author.GetAttribute("textContent");

                Console.WriteLine(str_title + ' ' + str_views_split[0] + ' ' + str_author);
            }
            driver.Close();
        }

        public void getJobs()
        {
            text = "";
            IWebDriver driver = startBrowser();
            Int32 vcount = 1;
            driver.Url = url;
            /* Explicit Wait to ensure that the page is loaded completely by reading the DOM state */
            var timeout = 10000; /* Maximum wait time of 10 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            Thread.Sleep(1000);
            
            IList<IWebElement> jobs;// = driver.FindElements(By.CssSelector(".job_seen_beacon"));
            try
            {

                string newurl;
                
                string JobsAmount = driver.FindElement(By.CssSelector("#searchCountPages")).Text.Substring(10);
                JobsAmount = Regex.Replace(JobsAmount, "[^0-9]+", string.Empty);
                int jobsAmountInt = int.Parse(JobsAmount);
                int remainder = jobsAmountInt % 15;
                if(remainder != 0)
                {
                    remainder = 1;
                }
                int pages = jobsAmountInt / 15 + remainder; //15 jobs per page
                
                for (int x = 0; x < pages; x++)
                {
                    jobs = driver.FindElements(By.CssSelector(".job_seen_beacon"));
                    foreach (IWebElement job in jobs)
                    {
                        string str_title, str_company, str_city, str_posted, csvtext;
                        IWebElement elem_job_title = job.FindElement(By.CssSelector(".resultContent div h2 span:nth-child(2)"));
                        str_title = elem_job_title.Text;
                        IWebElement elem_job_company = job.FindElement(By.CssSelector(".companyName"));
                        str_company = elem_job_company.Text;
                        IWebElement elem_job_city = job.FindElement(By.CssSelector(".companyLocation"));
                        str_city = elem_job_city.Text;
                        IWebElement elem_job_date = job.FindElement(By.CssSelector(".date"));
                        str_posted = elem_job_date.Text;
                        Console.WriteLine($"Job {vcount}");
                        Console.WriteLine(str_title);
                        Console.WriteLine(str_company);
                        Console.WriteLine(str_city);
                        Console.WriteLine(str_posted);
                        Console.WriteLine();
                        csvtext = $"{vcount};{str_title};{str_company};{str_city}";
                        generateCSV(csvtext);
                        vcount++;
                    }
                    newurl = url+$"&start={x * 10}";
                    driver.Navigate().GoToUrl(newurl);
                    Thread.Sleep(1000);
                }
                driver.Close();
            }
            catch (NoSuchElementException e)
            {
                Console.Clear();
                Console.WriteLine("Nothing found or invalid querry");
                driver.Close();
            }
        }
        
        public void generateCSV(string gentext)
        {
            text += gentext + Environment.NewLine;
        }

        public void writeCSV(string name)
        {
            File.WriteAllText(name, text);
        }
        
    }
}
// 437 lines!!