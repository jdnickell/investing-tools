using Microsoft.Extensions.Logging;
using Service.Services.AfterHoursServices.Models;
using Service.Services.ExtendedHoursServices;
using Service.Services.SymbolServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.ThirdPartyServices.DataServices.PolygonServices
{
    public class GetPostMarketBiggestMovers : IGetPostMarketBiggestMovers
    {
        private readonly IGetSymbols _getSymbols;
        private readonly IGetOpenClose _getOpenClose;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<GetPostMarketBiggestMovers> _logger;

        const int POSITIVE_GAIN_PERCENT_THRESHOLD = 7;

        public GetPostMarketBiggestMovers(IGetSymbols getSymbols
            , IGetOpenClose getOpenClose
            , IHttpClientFactory httpClientFactory
            , ILogger<GetPostMarketBiggestMovers> logger)
        {
            _getSymbols = getSymbols;
            _getOpenClose = getOpenClose;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<DailyOpenCloseResult>> GetListAsync(string openCloseDate)
        {
            var tickers = await _getSymbols.GetListAsync();
            var dailyOpenCloseResultAboveThreshold = new List<DailyOpenCloseResult>();
            var httpClient = _httpClientFactory.CreateClient();

            foreach (var ticker in tickers)
            {
                try
                {
                    var openClose = await _getOpenClose.GetAsync(ticker.Symbol1, openCloseDate, httpClient);

                    // skip those we couldn't get a result for
                    if (openClose == null) continue;

                    openClose.SymbolId = ticker.Id;

                    var percentChange = GetPercentChange(openClose.PriceClose, openClose.PriceAfterHours);

                    if (percentChange >= POSITIVE_GAIN_PERCENT_THRESHOLD)
                        dailyOpenCloseResultAboveThreshold.Add(openClose);
                }
                catch(Exception getOpenCloseException)
                {
                    _logger.LogError(getOpenCloseException, "Error getting open/close results");
                }
            }

            return dailyOpenCloseResultAboveThreshold;
        }

        private decimal GetPercentChange(decimal closePrice, decimal afterHoursPrice)
        {
            var priceChange = afterHoursPrice - closePrice;
            var percentChanged = (priceChange / closePrice) * 100;
            return percentChanged;
        }
    }
}
