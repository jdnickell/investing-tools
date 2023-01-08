using DataAccess;
using Service.Services.NotificationServices.Discord;
using System;
using System.Threading.Tasks;

namespace Service.Services.RecommendationServices
{
    public class CreateRecommendationCommand : ICreateRecommendationCommand
    {
        private readonly TradesContext _tradesContext;
        private readonly ISendDiscordMessageCommand _sendDiscordMessageCommand;

        public CreateRecommendationCommand(TradesContext tradesContext, 
            ISendDiscordMessageCommand sendDiscordMessageCommand)
        {
            _tradesContext = tradesContext;
            _sendDiscordMessageCommand = sendDiscordMessageCommand;
        }

        public async Task ExecuteAsync(string ticker, decimal currentPrice, Enums.RecommendationActionType recommendationActionType)
        {
            try
            {
                var recommendation = new Recommendation
                {
                    Ticker = ticker,
                    CurrentPrice = currentPrice,
                    EventDateTime = DateTime.UtcNow,
                    RecommendationTypeId = (int)recommendationActionType
                };

                await _tradesContext.AddAsync(recommendation);
                await _tradesContext.SaveChangesAsync();

                //todo: get a formatted message template for message types
                var typeName = Enum.GetName(typeof(Enums.RecommendationActionType), (int)recommendationActionType);
                await _sendDiscordMessageCommand.Execute($"Recommendation to {typeName} {ticker} at ${currentPrice}.");
            }
            catch(Exception ex)
            {
                //todo: Logging
            }
        }
    }

    public interface ICreateRecommendationCommand
    {
        Task ExecuteAsync(string ticker, decimal currentPrice, Enums.RecommendationActionType recommendationActionType);
    }
}
