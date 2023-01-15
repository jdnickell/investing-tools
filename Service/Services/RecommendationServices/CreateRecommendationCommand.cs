using DataAccess;
using Microsoft.Extensions.Logging;
using Service.Services.NotificationServices.Discord;
using System;
using System.Threading.Tasks;

namespace Service.Services.RecommendationServices
{
    public class CreateRecommendationCommand : ICreateRecommendationCommand
    {
        private readonly TradesContext _tradesContext;
        private readonly ILogger<CreateRecommendationCommand> _logger;
        private readonly ISendDiscordMessageCommand _sendDiscordMessageCommand;

        public CreateRecommendationCommand(TradesContext tradesContext
            , ILogger<CreateRecommendationCommand> logger
            , ISendDiscordMessageCommand sendDiscordMessageCommand)
        {
            _tradesContext = tradesContext;
            _logger = logger;
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
            }
            catch (Exception createRecommendationCommandException)
            {
                _logger.LogError(createRecommendationCommandException, "Error saving recommendation for ticker {Ticker}", ticker);
            }

            try
            {
                //todo: get a formatted message template for message types
                var typeName = Enum.GetName(typeof(Enums.RecommendationActionType), (int)recommendationActionType);
                await _sendDiscordMessageCommand.Execute($"Recommendation to {typeName} {ticker} at ${currentPrice}.");
            }
            catch (Exception sendDiscordMessageException)
            {
                _logger.LogError(sendDiscordMessageException, "Error sending Discord recommendation message for ticker {Ticker}", ticker);
            }
        }
    }

    public interface ICreateRecommendationCommand
    {
        Task ExecuteAsync(string ticker, decimal currentPrice, Enums.RecommendationActionType recommendationActionType);
    }
}
