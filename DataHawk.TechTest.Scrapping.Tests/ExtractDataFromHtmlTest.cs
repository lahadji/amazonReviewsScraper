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
    public class ExtractDataFromHtmlTest
    {
        //TODO : need to test on page without comments
        [TestMethod]
        public void CheckGetNumberOfComment()
        {
            String htmlData = File.ReadAllText($"HtmlData/FullPageOfReview.html");
            DataHawk.TechTest.Scrapping.Scrapper scrapper = new Scrapper();
            Int32 nbComments = scrapper.GetNbComments(htmlData);

            Check.That(nbComments).Equals(86);


        }
    }
}