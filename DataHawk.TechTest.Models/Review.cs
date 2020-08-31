using System;

namespace DataHawk.TechTest.Models
{
    public class Review
    {
        public string title;
        public string comment;
        public string author;
        public int nbPeopleFindHelpul;
        public bool verifiedPurchase;
        public DateTime reviewDate;
        public int nbComment;
        public string id { get; set; }
    }
}