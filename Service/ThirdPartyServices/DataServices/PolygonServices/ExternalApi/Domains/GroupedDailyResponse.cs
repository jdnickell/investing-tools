using System.Collections.Generic;

namespace Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi.Domains
{
    public class GroupedDailyResponse
    {
        /// <summary>
        /// Whether or not this response was adjusted for splits.
        /// </summary>
        public bool adjusted { get; set; }
        /// <summary>
        /// The number of aggregates (minute or day) used to generate the response.
        /// </summary>
        public int queryCount { get; set; }
        /// <summary>
        /// List of <see cref="GroupedDailyResults"/>
        /// </summary>
        public List<GroupedDailyResults> results { get; set; } = new List<GroupedDailyResults>();
    }

    /// <summary>
    /// Individual ticker detailed result objects
    /// </summary>
    public class GroupedDailyResults
    {
        /// <summary>
        /// The exchange symbol that this item is traded under.
        /// </summary>
        public string T { get; set; }
        /// <summary>
        /// The close price for the symbol in the given time period.
        /// </summary>
        public decimal c { get; set; }
        /// <summary>
        /// The highest price for the symbol in the given time period.
        /// </summary>
        public decimal h { get; set; }
        /// <summary>
        /// The lowest price for the symbol in the given time period.
        /// </summary>
        public decimal l { get; set; }
        /// <summary>
        /// The number of transactions in the aggregate window.
        /// </summary>
        public decimal n { get; set; }
        /// <summary>
        /// The open price for the symbol in the given time period.
        /// </summary>
        public decimal o { get; set; }
        /// <summary>
        /// The Unix Msec timestamp for the start of the aggregate window.
        /// </summary>
        public string t { get; set; }
        /// <summary>
        /// The trading volume of the symbol in the given time period.
        /// </summary>
        public decimal v { get; set; }
        /// <summary>
        /// The volume weighted average price.
        /// </summary>
        public float vw { get; set; }
    }
}
