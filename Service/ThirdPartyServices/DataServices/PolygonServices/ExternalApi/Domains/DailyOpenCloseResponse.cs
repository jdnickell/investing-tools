namespace Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi.Domains
{
    public class DailyOpenCloseResponse
    {
        public string status { get; set; }
        public string from { get; set; }
        public string symbol { get; set; }
        public float open { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float close { get; set; }
        public float volume { get; set; }
        public float afterHours { get; set; }
        public float preMarket { get; set; }
    }
}
