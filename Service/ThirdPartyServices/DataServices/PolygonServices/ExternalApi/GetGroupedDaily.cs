using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Services.ConfigurationServices;
using Service.Services.ExtendedHoursServices;
using Service.Services.ExtendedHoursServices.Models;
using Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi.Domains;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi
{
    public class GetGroupedDaily : IGetGroupedDaily
    {
        private readonly IIsBiggestMover _isBiggestMover;
        private readonly ILogger<GetGroupedDaily> _logger;

        private static readonly bool UNADJUSTED_FOR_SPLIT = true;

        public GetGroupedDaily(ILogger<GetGroupedDaily> logger
            , IIsBiggestMover isBiggestMover)
        {
            _isBiggestMover = isBiggestMover;
            _logger = logger;
        }

        public async Task<List<GroupedDailyResult>> GetListAsync(string marketDate, HttpClient httpClient)
        {

            var apiUrl = $"{PolygonSettings.BaseApiUrl}/{Resources.Urls.GroupedDaily}/{marketDate}?unadjusted={UNADJUSTED_FOR_SPLIT}&apiKey={PolygonSettings.ApiKey}";

            var response = await httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request for GroupedDaily failed for market date {MarketDate} with status code {StatusCode}", marketDate, response.StatusCode);
                return null;
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            var groupedDailyResponses = JsonConvert.DeserializeObject<GroupedDailyResponse>(apiResponse);

            var groupedDailyResults = new List<GroupedDailyResult>();

            foreach (var groupedDailyResult in groupedDailyResponses.results)
            {
                if (!_isBiggestMover.Is(groupedDailyResult.c, groupedDailyResult.h))
                {
                    continue;
                }
                //TODO: define additional parameters to narrow down this list before making the GetOpenClose request.

                groupedDailyResults.Add(new GroupedDailyResult
                {
                    PriceClose = groupedDailyResult.c,
                    PriceHigh = groupedDailyResult.h,
                    PriceLow = groupedDailyResult.l,
                    PriceOpen = groupedDailyResult.o,
                    PriceWeightedAverage = groupedDailyResult.vw,
                    Symbol = groupedDailyResult.T,
                    SymbolId = 0, // TODO At the end, map this from db so we only query relevant ones
                    TransactionsCount = groupedDailyResult.n,
                    Volume = groupedDailyResult.v
                });
            }

            //TODO - set the SymbolId
            return groupedDailyResults;
        }
    }
}
