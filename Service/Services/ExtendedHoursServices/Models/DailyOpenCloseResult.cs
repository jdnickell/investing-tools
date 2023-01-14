using System;

namespace Service.Services.AfterHoursServices.Models
{
    public class DailyOpenCloseResult
    {
        public DateTime RequestedDate { get; set; }
        public string Symbol { get; set; }
        public int SymbolId { get; set; }
        public decimal PriceOpen { get; set; }
        public decimal PriceHigh { get; set; }
        public decimal PriceLow { get; set; }
        public decimal PriceClose { get; set; }
        public decimal PriceAfterHours { get; set; }
        public decimal PricePreMarket { get; set; }
        public decimal Volume { get; set; }
    }
}
