using System;
using System.Collections.Generic;
using System.Text;
using AngleSharp;
using DataHawk.TechTest.Models;

namespace DataHawk.TechTest.Scrapping
{
    public class Crawler
    {
        private Scrapper scrapper;

        public Crawler(Scrapper scrapper)
        {
            this.scrapper = scrapper;
        }

        //main
        public void GetComments(String url)
        {
            List<string> urlsToCrawl = this.GetUrlsToCrawl(url);

            List<DataHawk.TechTest.Models.Review> reviews = new List<Review>();

            foreach (string u in urlsToCrawl)
            {
                reviews.AddRange(scrapper.GetReviewFromHtmlPage(this.GetHtmlContent(u)));
            }
        }

        public List<String> GetUrlsToCrawl(string baseUrl)
        {
            if (!Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
            {
                //TODO : use regex to check format of produict id 
                //B082XY23D5
                //https://www.amazon.com/product-reviews/B082XY23D5

                baseUrl = $"https://www.amazon.com/product-reviews/{baseUrl}";
            }

            //TODO : use injection of dependency to use scrapper
            Scrapper scrapper = new Scrapper();
            int nbComments = scrapper.GetNbComments(this.GetHtmlContent(baseUrl));


            //Check of scrapping if it's only 10 comments by page
            Int32 nbPageOfComment = nbComments % 10;

            List<string> urlToCrawl = new List<string>();

            String patternPage = "?pageNumber=";
            for (int i = 0; i < nbPageOfComment; i++)
            {
                urlToCrawl.Add($"{baseUrl}/{patternPage}{i}");
            }

            return urlToCrawl;
        }

        public string GetHtmlContent(String url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            //TODO : use async !
            var document = context.OpenAsync(url).Result;

            //Maybe need to use user-agent / proxy for bypass amazon robot check
            if (string.Equals(document.Title, "Robot Check", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Unable to load page {url} robot check verification");
            }

            string documentTextContent = document.TextContent;
            return documentTextContent;
        }
    }
}
