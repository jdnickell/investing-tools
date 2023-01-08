using Newtonsoft.Json;
using Service.Services.ConfigurationServices;
using Service.Services.SymbolServices;
using Service.Services.SymbolServices.Models;
using Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi.Domains;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.ThirdPartyServices.DataServices.PolygonServices
{
    public class GetStockTicker : IGetStockTicker
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly string FILTER_AND_SORTING = "?type=CS&active=true&sort=ticker&order=asc&limit=1000";

        public GetStockTicker(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Gets all StockTickerResults for supported stocks
        /// Todos: 
        /// -- Add mapping command objects to map between domain / externalApi domain
        /// -- Exception handling
        /// -- Probably make a generic Polygon client/webcall to use
        /// -- This list doesn't change often, so we could store it somewhere and then just use this method to seed / update the list as needed 
        /// </summary>
        /// <returns></returns>
        public async Task<List<StockTickerResult>> GetAllAsync()
        {
            var stockTickerResults = new List<StockTickerResult>();
            var httpClient = _httpClientFactory.CreateClient();
            string apiUrl = $"{PolygonSettings.BaseApiUrl}/{Resources.Urls.Tickers}{FILTER_AND_SORTING}";

            while(apiUrl != null)
            {
                //there are more results as long as next_url is not null, chain call with this appending &yourapikey
                var response = await httpClient.GetAsync($"{apiUrl}&apiKey={PolygonSettings.ApiKey}");
                var tickersApiResponse = await response.Content.ReadAsStringAsync();
                var tickersResponse = JsonConvert.DeserializeObject<TickersResponse>(tickersApiResponse);
                apiUrl = tickersResponse.next_url;

                foreach (var tickersResult in tickersResponse.results)
                {
                    stockTickerResults.Add(new StockTickerResult
                    {
                        Currency = tickersResult.currency_name,
                        LastUpdateDateTimeUtc = tickersResult.last_updated_utc,
                        Locale = tickersResult.locale,
                        Name = tickersResult.name,
                        PrimaryExchange = tickersResult.primary_exchange,
                        Symbol = tickersResult.ticker
                    });
                }
            }

            return stockTickerResults;
        }

        /// <summary>
        /// Get a StockTickerResult for a specific symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public Task<StockTickerResult> GetAsync(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
