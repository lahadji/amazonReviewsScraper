using System;
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
        public Review extractComment(string data)
        {
            Review result = new Review();

            //HtmlParser parser = new HtmlParser();
            //IHtmlDocument htmlDocument = parser.ParseDocument(data);

            var dom = new HtmlParser().ParseDocument(data);


            var titleElement = dom.QuerySelector("#customer_review-R3PFFVSQGIS9J6 > div:nth-child(2) > a.a-size-base.a-link-normal.review-title.a-color-base.review-title-content.a-text-bold > span");
            var contentElement =
                dom.QuerySelector(
                    "#customer_review-R3PFFVSQGIS9J6 > div.a-row.a-spacing-small.review-data > span > span");
            var authorElement =
                dom.QuerySelector(
                    "#customer_review-R3PFFVSQGIS9J6 > div:nth-child(1) > a > div.a-profile-content > span");
            var peopleFindHelpfulElement =
                dom.QuerySelector(
                    "#customer_review-R3PFFVSQGIS9J6 > div.a-row.review-comments.comments-for-R3PFFVSQGIS9J6 > div > span.cr-vote > div.a-row.a-spacing-small > span");
            var verifiedPurchaseElement =
                dom.QuerySelector(
                    "#customer_review-R3PFFVSQGIS9J6 > div.a-row.a-spacing-mini.review-data.review-format-strip > span > a");
            var reviewDateElement = dom.QuerySelector("#customer_review-R3PFFVSQGIS9J6 > span");
            var nbComment =
                dom.QuerySelector(
                    "#customer_review-R3PFFVSQGIS9J6 > div.a-row.review-comments.comments-for-R3PFFVSQGIS9J6 > div > a > span > span.a-size-base");


            result.title = titleElement.TextContent;
            result.comment = contentElement.TextContent.TrimStart().TrimEnd();
            result.author = authorElement.TextContent;
            result.nbPeopleFindHelpul = this.extractInt32FromString(peopleFindHelpfulElement.TextContent);
            result.verifiedPurchase = !String.IsNullOrEmpty(verifiedPurchaseElement.TextContent);
            String dateOfReviewString = reviewDateElement.TextContent;
            result.reviewDate =
                extractYearMonthFromString(dateOfReviewString.Substring(dateOfReviewString.LastIndexOf("on") + 2));

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