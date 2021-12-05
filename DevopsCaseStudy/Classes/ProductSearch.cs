using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace DevopsCaseStudy {
    public class ProductSearch {
        private string searchQuery, searchUrl, csvtext;
        private List<string> Stores = new List<string>{"Bol.com", "Amazon", "Coolblue", "Ebay"};
        private CSV csv = new CSV();

        public ProductSearch(string searchQuery) {
            this.searchQuery = searchQuery;
        }

        private void printProducts(string store, string name, string link, string price) {
            Console.WriteLine("******* " + store + " *******");
            Console.WriteLine("Product Title: " + name);
            Console.WriteLine("Product Price: " + price);
            Console.WriteLine("Produxt Link: " + link);
            Console.WriteLine("");
        }

        public void searchProducts(Browser browser) {
            IWebDriver driver = browser.startBrowser();
            String csvheader = "Store;Title;Price;Link";
            csv.generateCSV(csvheader);
            
            foreach (var store in Stores) {
                if (store == "Bol.com") {
                    try {
                        searchUrl = "https://www.bol.com/nl/nl/s/?searchtext=" + searchQuery.Replace(" ", "+");
                        driver.Url = searchUrl;
                        var timeout = 10000;
                        var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                        String productName, productPrice, productLink;
                        productName = driver.FindElement(By.CssSelector(".product-item--row .product-title--inline a")).Text;
                        productLink = driver.FindElement(By.CssSelector(".product-item--row .product-title--inline a")).GetAttribute("href");
                        productPrice = driver.FindElement(By.CssSelector(".product-item--row .product-item__options .promo-price")).Text;
                        productPrice = productPrice.Replace("\r\n", ",");
                        printProducts("Bol.com", productName, productLink, productPrice);
                    
                        csvtext = $"\"Bol.com\";\"{productName}\";{productPrice};\"{productLink}\"";
                        csv.generateCSV(csvtext);
                    }
                    catch (NoSuchElementException e) {
                        Console.WriteLine("Nothing found");
                        csvtext = $"\"Bol.com\";\"Nothing found\"";
                        csv.generateCSV(csvtext);
                    }
                    
                }
                else if (store == "Amazon") {
                    try {
                        searchUrl = "https://www.amazon.nl/s?k=" + searchQuery.Replace(" ", "+");
                        driver.Url = searchUrl;
                        var timeout = 10000;
                        var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
                    
                        String productName, productPrice, productLink;
                        productName = driver.FindElement(By.CssSelector(".s-result-item h2 a")).Text;
                        productLink = driver.FindElement(By.CssSelector(".s-result-item h2 a")).GetAttribute("href");
                        productPrice = driver.FindElement(By.CssSelector("span .a-price-whole")).Text;
                        printProducts("Amazon", productName, productLink, productPrice);
                    
                        csvtext = $"\"Amazon\";\"{productName}\";{productPrice};\"{productLink}\"";
                        csv.generateCSV(csvtext); 
                    }
                    catch (NoSuchElementException e) {
                        Console.WriteLine("Nothing found");
                        csvtext = $"\"Amazon\";\"Nothing found\"";
                        csv.generateCSV(csvtext);
                    }
                    
                }
                else if (store == "Coolblue") {
                    try { 
                        searchUrl = "https://www.coolblue.be/nl/zoeken?query=" + searchQuery.Replace(" ", "+");
                        driver.Url = searchUrl;
                        var timeout = 10000;
                        var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
                    
                        String productName, productPrice, productLink;
                        productName = driver.FindElement(By.CssSelector(".product-card .product-card__title a")).Text;
                        productLink = driver.FindElement(By.CssSelector(".product-card .product-card__title a")).GetAttribute("href");
                        productPrice = driver.FindElement(By.CssSelector(".product-card .sales-price__current")).Text;
                        productPrice = productPrice.Replace("-", "00");
                        printProducts("Coolblue", productName, productLink, productPrice);
                    
                        csvtext = $"\"Coolblue\";\"{productName}\";{productPrice};\"{productLink}\"";
                        csv.generateCSV(csvtext);
                    }
                    catch (NoSuchElementException e) {
                        Console.WriteLine("Nothing found");
                        csvtext = $"\"Coolblue\";\"Nothing found\"";
                        csv.generateCSV(csvtext);
                    }
                    
                }
                else if (store == "Ebay") {
                    try {
                        searchUrl = "https://www.benl.ebay.be/sch/i.html?_from=R40&_nkw=" + searchQuery.Replace(" ", "+");
                        driver.Url = searchUrl;
                        var timeout = 10000;
                        var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
                        
                        String productName, productPrice, productLink;
                        productName = driver.FindElement(By.CssSelector("#srp-river-results .s-item .s-item__title")).Text;
                        productLink = driver.FindElement(By.CssSelector("#srp-river-results .s-item .s-item__link")).GetAttribute("href");
                        productPrice = driver.FindElement(By.CssSelector("#srp-river-results .s-item .s-item__price")).Text;
                        productPrice = productPrice.Replace("EUR ", "");
                        printProducts("Ebay", productName, productLink, productPrice);
                    
                        csvtext = $"\"Ebay\";\"{productName}\";{productPrice};\"{productLink}\"";
                        csv.generateCSV(csvtext);
                    }
                    catch (NoSuchElementException e) {
                        Console.WriteLine("Nothing found");
                        csvtext = $"\"Ebay\";\"Nothing found\"";
                        csv.generateCSV(csvtext);
                    }
                }
            }
            driver.Close();
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
        public void writeCSV() {
            csv.writeCSV("product search -  " + this.searchQuery + " " + DateTime.Now.ToString("MM-dd-yyyy HH.mm.ss"));
        }
    }
}