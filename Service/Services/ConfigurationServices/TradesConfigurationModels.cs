namespace Service.Services.ConfigurationServices
{
    public static class ConnectionStrings
    {
        public static string TradesConnectionString { get; set; }
    }

    public static class NotificationSettings
    {
        public static bool IsDiscordRecommendationsEnabled { get; set; }
        public static string DiscordRecommendationWebhookId { get; set; }
    }

    public static class PolygonSettings
    {
        public static bool IsTestMode { get; set; }
        public static string ApiKey { get; set; }
        public static string BaseApiUrl { get; set; }
    }
}