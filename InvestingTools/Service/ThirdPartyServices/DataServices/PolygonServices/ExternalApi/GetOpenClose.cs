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

        //When true, results are NOT adjusted for splits
        private static readonly bool unadjustedForSplit = true;

        public GetOpenClose(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<DailyOpenCloseResult> GetAsync(string symbol, string openCloseDate)
        {
            string apiUrl = $"{PolygonSettings.BaseApiUrl}/{Resources.Urls.OpenClose}/{symbol}/{openCloseDate}?unadjusted={unadjustedForSplit}&apiKey={PolygonSettings.ApiKey}";

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(apiUrl);
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
                Volume = dailyOpenCloseResponse.volume
            };

            return dailyOpenCloseResult;
        }
    }
}
