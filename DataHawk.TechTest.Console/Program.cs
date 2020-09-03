using System;
using System.IO;
using DataHawk.TechTest.Scrapping;

namespace DataHawk.TechTest.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Crawler crawler = new Crawler(new Scrapper());

            crawler.GetComments("https://www.amazon.com/product-reviews/B082XY23D5");

        //    DataHawk.TechTest.Scrapping.Scrapper scrapper = new Scrapper();

        //    scrapper.GetNbComments(
        //        File.ReadAllText(@"C:\_Perso\DataHawk.TechTest.Scrapping.Tests\HtmlData\FullPageOfReview.html"));
        }
    }
}
