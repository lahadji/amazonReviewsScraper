using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AngleSharp.Dom;
using NFluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataHawk.TechTest.Models;
using DataHawk.TechTest.Scrapping;


namespace DataHawk.TechTest.Scrapping.Tests
{
    [TestClass]
    public class ExtractCommentFromHtmlTest
    {
        //URL : https://www.amazon.com/product-reviews/B082XY23D5

        [TestMethod]
        public void TestGetHtmlData()
        {
            String htmlData = File.ReadAllText($"HtmlData/OneComment.html");

            Check.That(htmlData).IsNotNull();
            Check.That(htmlData).IsNotEmpty();

            //<div id="R3PFFVSQGIS9J6"
            Check.That(htmlData).StartsWith("<div id=\"R3PFFVSQGIS9J6\"");
        }

        [TestMethod]
        public void ExtractOneCommentFromHtml()
        {
            String htmlData = File.ReadAllText($"HtmlData/OneComment.html");
            DataHawk.TechTest.Scrapping.Scrapper scrapper = new Scrapper();
            Review review = scrapper.extractComment(htmlData);

            Check.That(review.title).Equals("FM Radio still not active in the US unlocked version");

            Check.That(review.comment).StartsWith("Despite Samsung's promises,");
            Check.That(review.comment).EndsWith("or working update real soon. *");

            Check.That(review.author).Equals("Ta");
            Check.That(review.nbPeopleFindHelpul).Equals(158);

            Check.That(review.verifiedPurchase).Equals(true);

            var expectedDate = new DateTime(2020,3,1);
            Check.That(review.reviewDate).IsInSameYearAs(expectedDate).And.IsInSameMonthAs(expectedDate);

            Check.That(review.nbComment).Equals(13);
        }


        [TestMethod]
        public void CheckThat2CommentsOnHtml()
        {
            String htmlData = File.ReadAllText($"HtmlData/TwoComments.html");

            DataHawk.TechTest.Scrapping.Scrapper scrapper = new Scrapper();

            List<IElement> listOfHtmlComments =  scrapper.GetListOfHtmlComment(htmlData);

            Check.That(listOfHtmlComments).HasSize(2);
        }

        [TestMethod]
        public void CheckOn2HtmlCommentFirstComment()
        {
            String htmlData = File.ReadAllText($"HtmlData/TwoComments.html");
            DataHawk.TechTest.Scrapping.Scrapper scrapper = new Scrapper();
            List<IElement> listOfHtmlComments = scrapper.GetListOfHtmlComment(htmlData);

            var review = scrapper.extractComment(listOfHtmlComments.First().OuterHtml);

            Check.That(review.title).Equals("FM Radio still not active in the US unlocked version");

            Check.That(review.comment).StartsWith("Despite Samsung's promises,");
            Check.That(review.comment).EndsWith("or working update real soon. *");

            Check.That(review.author).Equals("Ta");
            Check.That(review.nbPeopleFindHelpul).Equals(159);

            Check.That(review.verifiedPurchase).Equals(true);

            var expectedDate = new DateTime(2020, 3, 1);
            Check.That(review.reviewDate).IsInSameYearAs(expectedDate).And.IsInSameMonthAs(expectedDate);

            Check.That(review.nbComment).Equals(13);
        }

        [TestMethod]
        public void CheckOn2HtmlCommentSecondComment()
        {
            String htmlData = File.ReadAllText($"HtmlData/TwoComments.html");
            DataHawk.TechTest.Scrapping.Scrapper scrapper = new Scrapper();
            List<IElement> listOfHtmlComments = scrapper.GetListOfHtmlComment(htmlData);

            var review = scrapper.extractComment(listOfHtmlComments[1].OuterHtml);

            Check.That(review.title).Equals("Incomplete shipment");

            Check.That(review.comment).StartsWith("Didn't come with the offered Buds");
            Check.That(review.comment).EndsWith("What's up!!");

            Check.That(review.author).Equals("Ricardo Wagner");
            Check.That(review.nbPeopleFindHelpul).Equals(134);

            Check.That(review.verifiedPurchase).Equals(true);

            var expectedDate = new DateTime(2020, 10, 1);
            Check.That(review.reviewDate).IsInSameYearAs(expectedDate).And.IsInSameMonthAs(expectedDate);

            Check.That(review.nbComment).Equals(3);
        }

        //TODO need to find review without verifiedPurchase
    }
}