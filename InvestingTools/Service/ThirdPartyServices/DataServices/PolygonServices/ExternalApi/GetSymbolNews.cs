using Newtonsoft.Json;
using Service.Services.ConfigurationServices;
using Service.Services.SentimentServices.NewsServices;
using Service.Services.SentimentServices.NewsServices.Models;
using Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi.Domains;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi
{
    public class GetSymbolNews : IGetSymbolNews
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly string LIMIT_AND_SORT_ORDER = "limit=20&order=descending&sort=published_utc";

        public GetSymbolNews(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<NewsResult>> GetListAsync(string ticker, string greaterThanOrEqualPublishedDate)
        {
            var httpClient = _httpClientFactory.CreateClient();
            string apiUrl = $"{PolygonSettings.BaseApiUrl}/{Resources.Urls.TickerNews}?{LIMIT_AND_SORT_ORDER}&ticker={ticker}&published_utc.gte={greaterThanOrEqualPublishedDate}";

            var response = await httpClient.GetAsync($"{apiUrl}&apiKey={PolygonSettings.ApiKey}");

            var newsTickersApiResponse = await response.Content.ReadAsStringAsync();
            var tickerNewsResponse = JsonConvert.DeserializeObject<TickerNewsResponse>(newsTickersApiResponse);

            var newsResults = new List<NewsResult>();

            if (tickerNewsResponse.results == null)
                return newsResults;

            foreach (var tickerNewsResponseResult in tickerNewsResponse.results)
            {
                var newsResult = new NewsResult
                {
                    Author = tickerNewsResponseResult.author,
                    PublishedDateTime = tickerNewsResponseResult.published_utc,
                    PublisherName = tickerNewsResponseResult.publisher.name,
                    PublisherUrl = tickerNewsResponseResult.publisher.homepage_url,
                    RelevantSymbols = tickerNewsResponseResult.tickers,
                    Title = tickerNewsResponseResult.title,
                    Summary = tickerNewsResponseResult.description,
                    Symbol = ticker,
                    Url = tickerNewsResponseResult.article_url
                };

                newsResults.Add(newsResult);
            }

            return newsResults;
        }
    }
}
