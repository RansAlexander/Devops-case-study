using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

namespace ConsoleApp1 {
    public class Program {
        private static int index = 0;
        
        static void Main(string[] args)
        {
            string selectedBrowser = null;
            string selectedRemote;
            bool remote;
            while (true)
            {
                // select mode
                Console.WriteLine("1) Get all videos from channel");
                Console.WriteLine("2) Get most recent video's");
                Console.WriteLine("3) Search on indeed");
                Console.WriteLine("5) Exit");
                Console.Write("Make selection: ");
                String selectedMenuItem = Console.ReadLine();
                Console.Clear();
                
                //select remote or not
                Console.WriteLine("Want to use remote browser?");
                Console.WriteLine("1) Yes (may be slower)");
                Console.WriteLine("2) No, use local browser");
                Console.Write("Make selection: ");
                selectedRemote = Console.ReadLine();
                Console.Clear();
                if (selectedRemote == "1")
                {
                    remote = true;
                }
                else
                {
                    remote = false;
                    Console.WriteLine("What browser to use?");
                    Console.WriteLine("1) Chrome");
                    Console.WriteLine("2) Firefox");
                    Console.WriteLine("3) Edge");
                    Console.WriteLine("4) Safari");
                    Console.WriteLine("5) Opera");
                    Console.Write("Make selection: ");
                    selectedBrowser = Console.ReadLine();
                    switch (selectedBrowser)
                    {
                        case "1":
                            selectedBrowser = "Chrome";
                            break;
                        case "2":
                            selectedBrowser = "Firefox";
                            break;
                        case "3":
                            selectedBrowser = "Edge";
                            break;
                        case "4":
                            selectedBrowser = "Safare";
                            break;
                        case "5":
                            selectedBrowser = "Opera";
                            break;
                    }
                }
                Console.Clear();
                if (selectedMenuItem == "1") // Get all videos from channel
                {
                    Console.Write("Give channel url: ");
                    string userUrl = Console.ReadLine()+"/videos";
                    if (remote)
                    {
                        Browser browser = new Browser(userUrl, "Chrome", true);
                        browser.getAllVideos();
                        Console.Write("Save to CSV? (Y/N) ");
                        if (Console.ReadLine().ToUpper() == "Y")
                        {
                            browser.writeCSV($"Youtube channel {DateTime.Now.ToString("MM-dd-yyyy HH.mm.ss.csv")}");
                            Console.Write("Saved. Press enter to continue");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                    else
                    {
                        Browser browser = new Browser(userUrl, selectedBrowser, false);
                        browser.getAllVideos();
                        Console.Write("Save to CSV? (Y/N) ");
                        if (Console.ReadLine().ToUpper() == "Y")
                        {
                            browser.writeCSV("Youtube - ");
                            Console.Write("Saved. Press enter to continue");
                            Console.ReadLine();
                            Console.Clear();
                        }
                        
                    }
                }
                else if (selectedMenuItem == "2") // Get most recent video's
                {
                    Console.Write("Give search query: ");
                    string searchQuery = Console.ReadLine();
                    string userUrl = $"https://www.youtube.com/results?search_query={searchQuery}&sp=CAISAhAB";
                    Browser browser = new Browser(userUrl);
                    browser.getRecenVideos();
                    Console.Write("Save to CSV? (Y/N) ");
                    if (Console.ReadLine().ToUpper() == "Y")
                    {
                        browser.writeCSV($"Youtube - \"{searchQuery}\" {DateTime.Now.ToString("MM-dd-yyyy HH.mm.ss.csv")}");
                        Console.Write("Saved. Press enter to continue");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                else if(selectedMenuItem == "3")
                {
                    Console.Write("Give search query: ");
                    string searchTerm = Console.ReadLine();
                    searchTerm.Replace(" ", "+");
                    Console.Write("Give city: ");
                    string city = Console.ReadLine();
                    Console.Write("Give max radius: ");
                    string radius = Console.ReadLine();
                    string userUrl = $"https://be.indeed.com/jobs?q={searchTerm}&l={city}&radius={radius}&sort=date&fromage=3";
                    Browser browser = new Browser(userUrl);
                    browser.getJobs();
                    Console.Write("Save to CSV? (Y/N) ");
                    if (Console.ReadLine().ToUpper() == "Y")
                    {
                        browser.writeCSV($"Indeed {searchTerm} {city} " + DateTime.Now.ToString("MM-dd-yyyy HH.mm.ss") + ".csv");
                        Console.Write("Saved. Press enter to continue");
                        Console.ReadLine();
                        Console.Clear();
                    }

                }
                else if (selectedMenuItem == "5") // Exit
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid selection");
                }
            }
        }
    }
} 