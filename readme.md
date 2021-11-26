####CASE 5 : DEVELOPING A WEBSCRAPER USING C#-BASED SELENIUM BROWSER AUTOMATION
Selenium is the most popular web browser automation project. It is supported by many programming
languages, including C#, and emulates user interaction with a browser. This allows one to automate many user
tests, but there are many other possibilities.

One of those things is web scraping. It's up to you to get to know Selenium and
in this role build it into a Console-based web scraping tool. For inspiration:
https://www.lambdatest.com/blog/scraping-dynamic-web-pages

The intention is to have at least 3 scraping options
- Scraping the basic data (link, title of the video, uploader and number of
  views) of the 5 most recently uploaded Youtube videos based on a
  search term that the user of the scraping tool can enter
- Scraping the data on the jobsite be.indeed.com. Hereby the user of the scraping tool must be able to
  enter a search term, after which the data is retrieved of all jobs that have been entered under that
  search term in the last 3 days: Title, company, location, link.
- 1 self-selected site & data, but always based on a term entered by the user of the scraping tool

Furthermore, the following must be taken into account:
- The data is written via a Dapper ORM or to a .CSV file
- The app is in a GitHub Actions CI/CD pipeline where you can download it as an artifact .exe-file

Creative and technical additions you make are also mentioned and highlighted, as this results in a higher score