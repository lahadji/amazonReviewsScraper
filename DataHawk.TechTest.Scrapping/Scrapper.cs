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

        public Review extractComment(string data)
        {
            Review result = new Review();

            //HtmlParser parser = new HtmlParser();
            //IHtmlDocument htmlDocument = parser.ParseDocument(data);

            var dom = new HtmlParser().ParseDocument(data);

            var titleElement = dom.QuerySelector(".review-title");
            
            var contentElement = dom.QuerySelector(".review-text-content");
            var authorElement = dom.QuerySelector(".a-profile-name");
            var peopleFindHelpfulElement = dom.QuerySelector(".cr-vote");
            var verifiedPurchaseElement =
                dom.QuerySelector(
                    "div.a-row.a-spacing-mini.review-data.review-format-strip > span > a > span");
            var reviewDateElement = dom.QuerySelector(".review-date");
            var nbCommentElement = dom.QuerySelector(".review-comment-total");

            result.title = titleElement.TextContent.TrimStart().TrimEnd();
            result.comment = contentElement.TextContent.TrimStart().TrimEnd();
            result.author = authorElement.TextContent;
            result.nbPeopleFindHelpul = this.extractInt32FromString(peopleFindHelpfulElement.TextContent);
            result.verifiedPurchase = !String.IsNullOrEmpty(verifiedPurchaseElement.TextContent);
            String dateOfReviewString = reviewDateElement.TextContent;
            result.reviewDate =
                extractYearMonthFromString(dateOfReviewString.Substring(dateOfReviewString.LastIndexOf("on") + 2));

            result.nbComment = extractInt32FromString(nbCommentElement.TextContent);

            return result;
        }

        //Need to test but MVP ... so ... YOLO !
        private Int32 extractInt32FromString(String strToParse)
        {
            String resultString = Regex.Match(strToParse, @"\d+").Value;
            int extractedInt = Int32.Parse(resultString);

            return extractedInt;
        }

        /// <summary>
        /// Only Year + Month
        /// </summary>
        /// <param name="strToParse"></param>
        /// <returns></returns>
        //Need to test ....
        private DateTime extractYearMonthFromString(String strToParse)
        {
            DateTime date;
            DateTime.TryParse(strToParse, out date);
            return date;
        }


    }
}