using System;

namespace Service.Services.SentimentServices.NewsServices.Models
{
    public class NewsResult
    {
        public string Symbol { get; set; }
        public int SymbolId { get; set; }
        public string PublisherName { get; set; }
        public string PublisherUrl { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDateTime { get; set; }
        public string Url { get; set; }
        public string[] RelevantSymbols { get; set; }
    }
}
