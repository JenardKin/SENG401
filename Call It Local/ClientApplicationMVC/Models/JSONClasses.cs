using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientApplicationMVC.Models
{
    public class ResponseReview
    {
        public string response { get; set; }
        public Review[] reviews { get; set; }
    }
    public class Response
    {
        public string response { get; set; }
    }
    public class Review
    {
        public string companyName { get; set; }
        public string username { get; set; }
        public string review { get; set; }
        public int? stars { get; set; }
        public long? timestamp { get; set; }
    }
    public class Reviews
    {
        public Review review { get; set; }
    }
}