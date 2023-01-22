using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Services.ConfigurationServices;
using Service.Services.ExtendedHoursServices;
using Service.Services.NotificationServices.Discord;
using Service.Services.RecommendationServices;
using Service.Services.SentimentServices.NewsServices;
using Service.Services.SymbolServices;

namespace Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    AssignConfigurationSettings(configuration);
                    services.AddHttpClient();

                    services.AddDbContext<TradesContext>(options => options.UseSqlServer(ConnectionStrings.TradesConnectionString), ServiceLifetime.Singleton);

                    services.AddSingleton<IIsBiggestMover, IsBiggestMover>();
                    services.AddSingleton<ISendDiscordMessageCommand, SendDiscordMessageCommand>();

                    services.AddTransient<IGetSymbols, GetSymbols>();
                    services.AddTransient<ISeedSymbolsCommand, SeedSymbolsCommand>();
                    services.AddTransient<IGetSymbolSourceModel, GetSymbolSourceModel>();
                    services.AddTransient<ICreateRecommendationCommand, CreateRecommendationCommand>();
                    services.AddTransient<IProcessPostMarketBiggestMoversCommand, ProcessPostMarketBiggestMoversCommand>();

                    // our plan was to make this more generic in case we didn't want to use Polygon at some point
                    services.AddTransient<IGetStockTicker, Service.ThirdPartyServices.DataServices.PolygonServices.GetStockTicker>();
                    services.AddTransient<IGetOpenClose, Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi.GetOpenClose>();
                    services.AddTransient<IGetSymbolNews, Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi.GetSymbolNews>();
                    services.AddTransient<IGetGroupedDaily, Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi.GetGroupedDaily>();
                    services.AddTransient<IGetPostMarketBiggestMovers, Service.ThirdPartyServices.DataServices.PolygonServices.GetPostMarketBiggestMovers>();

                    services.AddHostedService<Worker>();
                });
        public static void AssignConfigurationSettings(IConfiguration configuration)
        {
            var connectionStringsSection = configuration.GetSection(nameof(ConnectionStrings));
            ConnectionStrings.TradesConnectionString = connectionStringsSection[nameof(ConnectionStrings.TradesConnectionString)];

            var notificationSettingsSection = configuration.GetSection(nameof(NotificationSettings));
            NotificationSettings.DiscordRecommendationWebhookId = notificationSettingsSection[nameof(NotificationSettings.DiscordRecommendationWebhookId)];
            bool.TryParse(notificationSettingsSection[nameof(NotificationSettings.IsDiscordRecommendationsEnabled)], out var isDiscordRecommendationsEnabled);
            NotificationSettings.IsDiscordRecommendationsEnabled = isDiscordRecommendationsEnabled;

            var polygonSettingsSection = configuration.GetSection(nameof(PolygonSettings));
            PolygonSettings.ApiKey = polygonSettingsSection[nameof(PolygonSettings.ApiKey)];
            PolygonSettings.BaseApiUrl = polygonSettingsSection[nameof(PolygonSettings.BaseApiUrl)];
            bool.TryParse(polygonSettingsSection[nameof(PolygonSettings.IsTestMode)], out var isPolygonTestMode);
            PolygonSettings.IsTestMode = isPolygonTestMode;
        }
    }
}
