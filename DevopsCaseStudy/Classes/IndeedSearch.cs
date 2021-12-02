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
    public class IndeedSearch {
        private string searchQuery, searchCity, searchRadius;
        private CSV csv = new CSV();

        public IndeedSearch(string searchQuery, string searchCity) {
            this.searchRadius = "50";
            this.searchQuery = searchQuery;
            this.searchCity = searchCity;
        }
        public IndeedSearch(string searchRadius, string searchQuery, string searchCity) {
            this.searchRadius = searchRadius;
            this.searchQuery = searchQuery;
            this.searchCity = searchCity;
        }

        public void searchJobs(Browser browser) {
            IWebDriver driver = browser.startBrowser();
            int vcount = 1;
            string searchUrl = "https://be.indeed.com/jobs?q=" + this.searchQuery + "&l=" + this.searchCity +
                               "&radius=" + this.searchRadius + "&fromage=3";
            driver.Url = searchUrl;
            var timeout = 10000; /* Maximum wait time of 10 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            Thread.Sleep(1000);
            IList<IWebElement> jobs;
            
            // a try to catch selenium error for if indeed returns nothing
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
                    jobs = driver.FindElements(By.CssSelector(".tapItem"));
                    foreach (IWebElement job in jobs)
                    {
                        string str_title, str_company, str_city, str_posted, str_link, csvtext;
                        // IWebElement elem_job_link = job.FindElement(By.CssSelector("a .tapItem"));
                        str_link = job.GetAttribute("href");
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
                        Console.WriteLine(str_link);
                        Console.WriteLine();
                        csvtext = $"\"{vcount}\";\"{str_title}\";\"{str_company}\";\"{str_city}\";\"{str_link}\"";
                        csv.generateCSV(csvtext);
                        vcount++;
                    }
                    newurl = searchUrl+$"&start={x * 10}";
                    driver.Navigate().GoToUrl(newurl);
                    Thread.Sleep(1000);
                }
                driver.Close();
                Console.Write("Press enter to continue");
                Console.ReadLine();
            }
            catch (NoSuchElementException e)
            {
                driver.Close();
                Console.Clear();
                Console.WriteLine("Nothing found or invalid querry");
            }
        }
        public void writeCSV() {
            csv.writeCSV("ineed " + this.searchQuery + " " + DateTime.Now.ToString("MM-dd-yyyy HH.mm.ss"));
        }
    }
}