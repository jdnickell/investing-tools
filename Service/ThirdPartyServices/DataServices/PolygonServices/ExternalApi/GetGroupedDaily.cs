using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Services.ConfigurationServices;
using Service.Services.ExtendedHoursServices;
using Service.Services.ExtendedHoursServices.Models;
using Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi.Domains;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.ThirdPartyServices.DataServices.PolygonServices.ExternalApi
{
    public class GetGroupedDaily : IGetGroupedDaily
    {
        private readonly ILogger<GetGroupedDaily> _logger;
        private static readonly bool unadjustedForSplit = true;

        public GetGroupedDaily(ILogger<GetGroupedDaily> logger)
        {
            _logger = logger;
        }

        public async Task<List<GroupedDailyResult>> GetListAsync(string marketDate, HttpClient httpClient)
        {

            var apiUrl = $"{PolygonSettings.BaseApiUrl}/{Resources.Urls.GroupedDaily}/{marketDate}?unadjusted={unadjustedForSplit}&apiKey={PolygonSettings.ApiKey}";

            var response = await httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request for GroupedDaily failed for market date {MarketDate} with status code {StatusCode}", marketDate, response.StatusCode);
                return null;
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            var dailyOpenCloseResponse = JsonConvert.DeserializeObject<GroupedDailyResponse>(apiResponse);

            //TODO
            //1: This model isn't quite right and deserializing fails, compare and fix the GroupedDailyResponse model
            //2: Then build a list of GroupedDailyResult from the GroupedDailyResponse
            //3: return the list
            throw new NotImplementedException();
        }
    }
}
