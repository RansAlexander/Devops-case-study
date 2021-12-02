using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Threading;

namespace DevopsCaseStudy {
    public class YoutubeChannel {
        private string channelUrl, channelName;
        private CSV csv = new CSV();

        public YoutubeChannel(string channelUrl) {
            this.channelUrl = channelUrl + "/videos";
        }
        public void getAllVideos(Browser browser) {
            IWebDriver driver = browser.startBrowser();
            driver.Url = this.channelUrl;
            var timeout = 10000;
            int vcount = 0;
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            
            Thread.Sleep(1000);
            String pageTitle=driver.Title;
            // google asks to agree with cookies or some dumb shit which fucks with selenium
            if (pageTitle == "Before you continue to YouTube") {
                driver.FindElement(By.XPath("//*[contains(text(), 'I agree')]")).Click();
                Thread.Sleep(1000);
            }
            
            Int64 last_height = (Int64)(((IJavaScriptExecutor)driver).ExecuteScript("return document.documentElement.scrollHeight"));
            
            while (true) {
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
            channelName = title.Substring(0, title.Length-10);
            Console.WriteLine("Page title is : " + title);
            Console.WriteLine("Total number of videos in " + this.channelUrl + " are " + videos.Count);

            // Go through the Videos List and scrap the same to get the attributes of the videos in the channel
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
                csv.generateCSV(csvtext);
                vcount++;
            }
            driver.Close();
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }

        public void writeCSV() {
            csv.writeCSV(channelName + " " + DateTime.Now.ToString("MM-dd-yyyy HH.mm.ss"));
        }
    }
}