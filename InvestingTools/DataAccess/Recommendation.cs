using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Recommendation
    {
        public int Id { get; set; }
        public int RecommendationTypeId { get; set; }
        public DateTime EventDateTime { get; set; }
        public decimal CurrentPrice { get; set; }
        public string Ticker { get; set; }
    }
}
