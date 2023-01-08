using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class ExtendedHoursBiggestMovers
    {
        public int Id { get; set; }
        public int SymbolId { get; set; }
        public decimal PriceAfterHours { get; set; }
        public decimal PriceOpen { get; set; }
        public decimal PriceClose { get; set; }
        public DateTime MarketDate { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }

        public virtual Symbol Symbol { get; set; }
    }
}
