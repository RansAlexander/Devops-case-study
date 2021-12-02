using System;

namespace DevopsCaseStudy {
    public class program {
        static Browser browser = new Browser();
        public static CSV csv = new CSV();
        private static string menuSelection;

        static void incorrectSelection() {
            Console.Clear();
            Console.WriteLine("You did not make a valid selection");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }

        static void Main(string[] args) {
            while (true) {
                Console.Clear();
                Console.WriteLine("1) Scrape videos from channel");
                Console.WriteLine("2) Get recent videos");
                Console.WriteLine("3) Search on indeed");
                Console.WriteLine("4) Search for products");
                // Console.WriteLine("5) Preferences");
                Console.Write("Make your selection: ");
                menuSelection = Console.ReadLine();
                
                // Scrape videos from specific youtube channel
                if (menuSelection == "1") {
                    Console.Clear();
                    Console.Write("Give channel url: ");
                    string channelUrl = Console.ReadLine();
                    YoutubeChannel channel = new YoutubeChannel(channelUrl);
                    channel.getAllVideos(browser);
                    Console.Write("Would you like to save this to .CSV? (Y/N) ");
                    //read input and save to csv if yes
                    if (Console.ReadLine().ToUpper() == "Y") {
                        channel.writeCSV();
                    }
                }
                else if (menuSelection == "2") {
                    Console.Clear();
                    Console.Write("Give search query: ");
                    string searchQuery = Console.ReadLine();
                    YoutubeSearch youtubesearch = new YoutubeSearch(searchQuery);
                    youtubesearch.searchVideos(browser);
                    Console.Write("Would you like to save this to .CSV? (Y/N) ");
                    //read input and save to csv if yes
                    if (Console.ReadLine().ToUpper() == "Y") {
                        youtubesearch.writeCSV();
                    }
                }
                else if (menuSelection == "3") {
                    Console.Clear();
                    Console.Write("Give search query: ");
                    string searchQuery = Console.ReadLine();
                    Console.Write("Give your city: ");
                    string searchCity = Console.ReadLine();
                    Console.Write("Give your city (Leave blanks for default 50m): ");
                    string searchRadius = Console.ReadLine();
                    if (searchRadius.Length > 0) {
                        IndeedSearch indeedsearch = new IndeedSearch(searchRadius, searchQuery, searchCity);
                        indeedsearch.searchJobs(browser);
                        Console.Write("Would you like to save this to .CSV? (Y/N) ");
                        if (Console.ReadLine().ToUpper() == "Y") {
                            indeedsearch.writeCSV();
                        }
                    }
                    else {
                        IndeedSearch indeedsearch = new IndeedSearch(searchQuery, searchCity);
                        indeedsearch.searchJobs(browser);
                        Console.Write("Would you like to save this to .CSV? (Y/N) ");
                        if (Console.ReadLine().ToUpper() == "Y") {
                            indeedsearch.writeCSV();
                        }
                    }
                }
                else if (menuSelection == "4") {
                    Console.Clear();
                    Console.Write("Give search query: ");
                    string searchQuery = Console.ReadLine();
                    ProductSearch product = new ProductSearch(searchQuery);
                    product.searchProducts(browser);
                    Console.Write("Would you like to save this to .CSV? (Y/N) ");
                    if (Console.ReadLine().ToUpper() == "Y") {
                        product.writeCSV();
                    }
                }
                
                // preferences
                else if (menuSelection == "5") {
                    while (true) {
                        Console.Clear();
                        Console.WriteLine("Current preferences: Browser - " + browser.getBrowserName() +
                                          " Remote - " + browser.getRemote());
                        Console.WriteLine("1) Change preferred browser (only for local connection)");
                        Console.WriteLine("2) Change remote (may be slower)");
                        Console.WriteLine("3) Return");
                        Console.Write("Make your selection: ");
                        menuSelection = Console.ReadLine();

                        if (menuSelection == "1") {
                            Console.Clear();
                            Console.WriteLine("1) Chrome");
                            Console.WriteLine("2) Firefox");
                            Console.WriteLine("3) Edge");
                            Console.WriteLine("4) Opera");
                            Console.WriteLine("5) Safari");
                            Console.Write("Make your selection: ");
                            menuSelection = Console.ReadLine();
                            switch (menuSelection) {
                                case "1":
                                    browser.setBrowserName("Chrome");
                                    break;
                                case "2":
                                    browser.setBrowserName("Firefox");
                                    break;
                                case "3":
                                    browser.setBrowserName("Edge");
                                    break;
                                case "4":
                                    browser.setBrowserName("Opera");
                                    break;
                                case "5":
                                    browser.setBrowserName("Safari");
                                    break;
                                default:
                                    incorrectSelection();
                                    break;
                            }
                        }
                        else if (menuSelection == "2") {
                            Console.Clear();
                            Console.WriteLine("1) Remote");
                            Console.WriteLine("2) Local");
                            Console.Write("Make your selection: ");
                            menuSelection = Console.ReadLine();
                            switch (menuSelection) {
                                case "1":
                                    browser.setRemote(true);
                                    break;
                                case "2":
                                    browser.setRemote(false);
                                    break;
                                default:
                                    incorrectSelection();
                                    break;
                            }
                        }
                        else if (menuSelection == "3") {
                            break;
                        }
                        else {
                            incorrectSelection();
                        }
                    }
                }
                else {
                    incorrectSelection();
                }
            }
        }
    }
}