using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Services.AfterHoursServices.Models;
using Service.Services.ConfigurationServices;
using Service.Services.ExtendedHoursServices;
using Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi.Domains;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi
{
    public class GetOpenClose : IGetOpenClose
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<GetOpenClose> _logger;

        //When true, results are NOT adjusted for splits
        private static readonly bool unadjustedForSplit = true;

        public GetOpenClose(IHttpClientFactory httpClientFactory
            , ILogger<GetOpenClose> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<DailyOpenCloseResult> GetAsync(string symbol, string openCloseDate)
        {
            string apiUrl = $"{PolygonSettings.BaseApiUrl}/{Resources.Urls.OpenClose}/{symbol}/{openCloseDate}?unadjusted={unadjustedForSplit}&apiKey={PolygonSettings.ApiKey}";

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogInformation("OpenClose not found for Ticker {Symbol}", symbol);
                return null;
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            var dailyOpenCloseResponse = JsonConvert.DeserializeObject<DailyOpenCloseResponse>(apiResponse);

            var dailyOpenCloseResult = new DailyOpenCloseResult
            {
                PriceAfterHours = (decimal)dailyOpenCloseResponse.afterHours,
                PriceClose = (decimal)dailyOpenCloseResponse.close,
                PriceHigh = (decimal)dailyOpenCloseResponse.high,
                PriceLow = (decimal)dailyOpenCloseResponse.low,
                PriceOpen = (decimal)dailyOpenCloseResponse.open,
                PricePreMarket = (decimal)dailyOpenCloseResponse.preMarket,
                RequestedDate = DateTime.ParseExact(dailyOpenCloseResponse.from, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Symbol = dailyOpenCloseResponse.symbol,
                Volume = (decimal)dailyOpenCloseResponse.volume
            };
            return dailyOpenCloseResult;
        }
    }
}
