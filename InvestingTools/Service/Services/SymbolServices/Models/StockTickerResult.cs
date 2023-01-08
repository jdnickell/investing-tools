using System;

namespace Service.Services.SymbolServices.Models
{
    public class StockTickerResult
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public string PrimaryExchange { get; set; }
        public string Currency { get; set; }
        public DateTime LastUpdateDateTimeUtc { get; set; }
    }
}
