using System;
using System.Threading.Tasks;

namespace Service.ThirdPartyServices.TradingServices.AlpacaServices
{
    public class MarketCalendarUtility : IMarketCalendarUtility
    {
        private IGetAlpacaClient _getAlpacaClient;
        public MarketCalendarUtility(IGetAlpacaClient getAlpacaClient)
        {
            _getAlpacaClient = getAlpacaClient;
        }
        public async Task<DateTime> GetMarketCloseDateTimeAsync()
        {
            var clock = await _getAlpacaClient.GetTradingClient().GetClockAsync();
            return clock.NextCloseUtc;
        }

        public async Task<DateTime> GetMarketOpenDateTimeAsync()
        {
            var clock = await _getAlpacaClient.GetTradingClient().GetClockAsync();
            return clock.NextOpenUtc;
        }

        public async Task<TimeSpan> GetTimespanUntilMarketCloseAsync()
        {
            var currentDate = DateTime.UtcNow;
            var nextMarkeClose = await GetMarketCloseDateTimeAsync();
            var timeUntilClose = nextMarkeClose - currentDate;
            return timeUntilClose;
        }

        public async Task<TimeSpan> GetTimespanUntilMarketOpenAsync()
        {
            var currentDate = DateTime.UtcNow;
            var nextMarkeOpen = await GetMarketOpenDateTimeAsync();
            var timeUntilOpen = nextMarkeOpen - currentDate;
            return timeUntilOpen;
        }

        public async Task<bool> IsMarketOpenAsync()
        {
            var clock = await _getAlpacaClient.GetTradingClient().GetClockAsync();
            return clock.IsOpen;
        }
    }
    public interface IMarketCalendarUtility
    {
        Task<bool> IsMarketOpenAsync();
        Task<DateTime> GetMarketOpenDateTimeAsync();
        Task<DateTime> GetMarketCloseDateTimeAsync();
        Task<TimeSpan> GetTimespanUntilMarketOpenAsync();
        Task<TimeSpan> GetTimespanUntilMarketCloseAsync();
    }
}
