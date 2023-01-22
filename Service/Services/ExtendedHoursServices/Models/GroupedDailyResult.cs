namespace Service.Services.ExtendedHoursServices.Models
{
    /// <summary>
    /// General daily price / transaction / volume information for a ticker
    /// </summary>
    public class GroupedDailyResult
    {
        /// <summary>
        /// Ticker Symobl
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// Symbol Id
        /// </summary>
        public int SymbolId { get; set; }
        /// <summary>
        /// The close price for the symbol in the given time period.
        /// </summary>
        public decimal PriceClose { get; set; }
        /// <summary>
        /// The highest price for the symbol in the given time period.
        /// </summary>
        public decimal PriceHigh { get; set; }
        /// <summary>
        /// The lowest price for the symbol in the given time period.
        /// </summary>
        public decimal PriceLow { get; set; }
        /// <summary>
        /// The open price for the symbol in the given time period.
        /// </summary>
        public decimal PriceOpen { get; set; }
        /// <summary>
        /// The volume weighted average price.
        /// </summary>
        public float PriceWeightedAverage { get; set; }
        /// <summary>
        /// The number of transactions in the aggregate window.
        /// </summary>
        public decimal TransactionsCount { get; set; }
        /// <summary>
        /// The trading volume of the symbol in the given time period.
        /// </summary>
        public decimal Volume { get; set; }
    }
}
