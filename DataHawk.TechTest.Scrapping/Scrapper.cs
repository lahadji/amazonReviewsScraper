using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Text;
using DataHawk.TechTest.Models;

namespace DataHawk.TechTest.Scrapping
{
    public class Scrapper
    {
        public List<IElement> GetListOfHtmlComment(string htmlData)
        {
            var dom = new HtmlParser().ParseDocument(htmlData);

            var list = dom.QuerySelectorAll(".aok-relative").ToList();

            return list;
        }

        public Review ExtractComment(string data)
        {
            Review result = new Review();

            var dom = new HtmlParser().ParseDocument(data);

            var titleElement = dom.QuerySelector(".review-title");
            var contentElement = dom.QuerySelector(".review-text-content");
            var authorElement = dom.QuerySelector(".a-profile-name");
            var peopleFindHelpfulElement = dom.QuerySelector(".cr-vote");
            var verifiedPurchaseElement = dom.QuerySelector("div.a-row.a-spacing-mini.review-data.review-format-strip > span > a > span");
            var reviewDateElement = dom.QuerySelector(".review-date");
            var nbCommentElement = dom.QuerySelector(".review-comment-total");
            var nbStar = dom.QuerySelector(".review-rating");

            
            result.Title = titleElement.TextContent.TrimStart().TrimEnd();
            result.Comment = contentElement.TextContent.TrimStart().TrimEnd();
            result.Author = authorElement.TextContent;
            result.NbPeopleFindHelpful = this.ExtractInt32FromString(peopleFindHelpfulElement.TextContent);
            result.VerifiedPurchase = !String.IsNullOrEmpty(verifiedPurchaseElement.TextContent);

            String dateOfReviewString = reviewDateElement.TextContent;
            dateOfReviewString = dateOfReviewString.Substring(dateOfReviewString.LastIndexOf("on") + 2);
            result.ReviewDate = ExtractYearMonthDayFromString(dateOfReviewString);

            result.NbComment = ExtractInt32FromString(nbCommentElement.TextContent);

            result.Star = (int)Char.GetNumericValue(nbStar.TextContent.First());

            return result;
        }

        //Need to test but MVP ... so ... YOLO !
        private Int32 ExtractInt32FromString(String strToParse)
        {
            String resultString = Regex.Match(strToParse, @"\d+").Value;
            int extractedInt = Int32.Parse(resultString);

            return extractedInt;
        }

        /// <summary>
        /// Only Year + Month + Day
        /// </summary>
        /// <param name="strToParse"></param>
        /// <returns></returns>
        //Need to test ....
        private DateTime ExtractYearMonthDayFromString(String strToParse)
        {
            DateTime date;
            DateTime.TryParse(strToParse, out date);
            return date;
        }


    }
}