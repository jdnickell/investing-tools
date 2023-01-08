using Newtonsoft.Json;
using Service.Services.ConfigurationServices;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.NotificationServices.Discord
{
    public class SendDiscordMessageCommand : ISendDiscordMessageCommand
    {
        private static readonly string DISCORD_USERNAME = "stonkbot";
        private static readonly string DISCORD_AVATAR_URL = string.Empty;
        private static readonly string DISCORD_WEBHOOK_URL = $"https://discord.com/api/webhooks/{NotificationSettings.DiscordRecommendationWebhookId}";
        private static readonly HttpClient _client = new();

        public async Task Execute(string message)
        {
            if (!NotificationSettings.IsDiscordRecommendationsEnabled)
                return;

            var discordMessageModel = new DiscordMessageModel { avatar_url = DISCORD_AVATAR_URL, content = message, username = DISCORD_USERNAME };
            var content = new StringContent(JsonConvert.SerializeObject(discordMessageModel), Encoding.UTF8, "application/json");
            await _client.PostAsync(DISCORD_WEBHOOK_URL, content);
        }
    }

    public interface ISendDiscordMessageCommand
    {
        Task Execute(string message);
    }
}
