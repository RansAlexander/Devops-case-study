using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

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
            String csvheader = "VideoID;Video title;Uploader;Views;Link";
            csv.generateCSV(csvheader);
            
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
                string str_title, str_views, str_author, csvtext, str_link;
                IWebElement elem_video_title = video.FindElement(By.CssSelector("#video-title"));
                str_title = elem_video_title.Text;
                
                IWebElement elem_video_views = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[1]"));
                str_views = elem_video_views.Text.Replace(" views", "").Replace(" view", "");
                str_views = str_views.Replace(".", "").Replace("K", "00").Replace("M", "00000");
                
                IWebElement elem_video_author = video.FindElement(By.CssSelector("#channel-name a"));
                str_author = elem_video_author.GetAttribute("textContent");
                
                IWebElement elem_video_link = video.FindElement(By.CssSelector("#video-title"));
                str_link = elem_video_link.GetAttribute("href");

                vcount++;
                
                //format views. This is stupid
                if (str_views == "No") {
                    str_views = "0";
                }
                
                Console.WriteLine("******* Video " + vcount + " *******");
                Console.WriteLine("Video Title: " + str_title);
                Console.WriteLine("Video Views: " + str_views + " views");
                Console.WriteLine("Video Author: " + str_author);
                Console.WriteLine("Video Link: " + str_link);
                Console.WriteLine("");
                
                csvtext = $"{vcount};\"{str_author}\";\"{str_title}\";{str_views};\"{str_link}\"";
                csv.generateCSV(csvtext);
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