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
    public class YoutubeSearch {
        private string searchQuery;
        private string searchTitle;
        private CSV csv = new CSV();
        
        public YoutubeSearch(string searchQuery) {
            searchTitle = searchQuery;
            this.searchQuery = "https://www.youtube.com/results?search_query=" + searchQuery + "&sp=CAI%253D";
        }

        public void searchVideos(Browser browser) {
            IWebDriver driver = browser.startBrowser();
            driver.Url = this.searchQuery;
            var timeout = 10000;
            int vcount = 0;
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            
            // Youtube asks for you to accept cookies or whatever bullshit so this is to check if it does and accept it
            driver.FindElement(By.XPath("//*[contains(text(), 'I Agree')]")).Click();
            ReadOnlyCollection<IWebElement> videos = driver.FindElements(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[2]/ytd-item-section-renderer/div[3]/ytd-video-renderer[position()<=5]"));
            foreach (IWebElement video in videos)
            {
                string str_title, str_views, str_author, csvtext;
                IWebElement elem_video_title = video.FindElement(By.CssSelector("#video-title"));
                str_title = elem_video_title.Text;
                
                IWebElement elem_video_views = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[1]"));
                str_views = elem_video_views.Text;
                string[] str_views_split = str_views.Split(" ");
                
                IWebElement elem_video_author = video.FindElement(By.CssSelector("#channel-name a"));
                str_author = elem_video_author.GetAttribute("textContent");

                vcount++;
                
                csvtext = $"\"{vcount}\";\"{str_author}\";\"{str_title}\";\"{str_views}\"";
                csv.generateCSV(csvtext);
                Console.WriteLine(str_title + ' ' + str_views_split[0] + ' ' + str_author);
            }
            driver.Close();
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
        public void writeCSV() {
            csv.writeCSV("Youtube search - " + searchTitle + " " + DateTime.Now.ToString("MM-dd-yyyy HH.mm.ss"));
        }
    }
}