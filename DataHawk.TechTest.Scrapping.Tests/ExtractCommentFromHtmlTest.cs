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
            Review review = scrapper.ExtractComment(htmlData);

            Check.That(review.Title).Equals("FM Radio still not active in the US unlocked version");

            Check.That(review.Comment).StartsWith("Despite Samsung's promises,");
            Check.That(review.Comment).EndsWith("or working update real soon. *");

            Check.That(review.Author).Equals("Ta");
            Check.That(review.NbPeopleFindHelpful).Equals(158);

            Check.That(review.VerifiedPurchase).Equals(true);

            var expectedDate = new DateTime(2020,3,8);
            Check.That(review.ReviewDate).
                IsInSameYearAs(expectedDate).And.
                IsInSameMonthAs(expectedDate).And.
                IsInSameDayAs((expectedDate));

            Check.That(review.NbComment).Equals(13);

            Check.That(review.Star).Equals(1);
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

            var review = scrapper.ExtractComment(listOfHtmlComments.First().OuterHtml);

            Check.That(review.Title).Equals("FM Radio still not active in the US unlocked version");

            Check.That(review.Comment).StartsWith("Despite Samsung's promises,");
            Check.That(review.Comment).EndsWith("or working update real soon. *");

            Check.That(review.Author).Equals("Ta");
            Check.That(review.NbPeopleFindHelpful).Equals(159);

            Check.That(review.VerifiedPurchase).Equals(true);

            var expectedDate = new DateTime(2020, 3, 8);
            Check.That(review.ReviewDate).
                IsInSameYearAs(expectedDate).And.
                IsInSameMonthAs(expectedDate).And.
                IsInSameDayAs(expectedDate);

            Check.That(review.NbComment).Equals(13);
            Check.That(review.Star).Equals(1);
        }

        [TestMethod]
        public void CheckOn2HtmlCommentSecondComment()
        {
            String htmlData = File.ReadAllText($"HtmlData/TwoComments.html");
            DataHawk.TechTest.Scrapping.Scrapper scrapper = new Scrapper();
            List<IElement> listOfHtmlComments = scrapper.GetListOfHtmlComment(htmlData);

            var review = scrapper.ExtractComment(listOfHtmlComments[1].OuterHtml);

            Check.That(review.Title).Equals("Incomplete shipment");

            Check.That(review.Comment).StartsWith("Didn't come with the offered Buds");
            Check.That(review.Comment).EndsWith("What's up!!");

            Check.That(review.Author).Equals("Ricardo Wagner");
            Check.That(review.NbPeopleFindHelpful).Equals(134);

            Check.That(review.VerifiedPurchase).Equals(true);

            var expectedDate = new DateTime(2020, 3, 10);
            Check.That(review.ReviewDate).
                IsInSameYearAs(expectedDate).And.
                IsInSameMonthAs(expectedDate).And.
                IsInSameDayAs(expectedDate);

            Check.That(review.NbComment).Equals(3);
            Check.That(review.Star).Equals(3);
        }

        [TestMethod]
        public void CheckFullPageOfReviewNbReview()
        {
            String htmlData = File.ReadAllText($"HtmlData/FullPageOfReview.html");

            DataHawk.TechTest.Scrapping.Scrapper scrapper = new Scrapper();

            List<IElement> listOfHtmlComments = scrapper.GetListOfHtmlComment(htmlData);

            Check.That(listOfHtmlComments).HasSize(10);
        }

        //TODO need to find review without verifiedPurchase
    }
}