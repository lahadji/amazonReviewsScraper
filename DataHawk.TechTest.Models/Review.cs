using System;

namespace DataHawk.TechTest.Models
{
    public class Review
    {
        public string Title;
        public string Comment;
        public string Author;
        public int NbPeopleFindHelpful;
        public bool VerifiedPurchase;
        
        public DateTime ReviewDate { get; set; }
        public int NbComment;
        public string Id;
        public Int32 Star { get; set; }
    }
}