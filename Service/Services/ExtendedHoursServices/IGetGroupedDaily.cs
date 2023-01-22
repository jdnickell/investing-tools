using Service.Services.ExtendedHoursServices.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.Services.ExtendedHoursServices
{
    /// <summary>
    /// Get the daily open, high, low, and close (OHLC) for the entire stocks/equities markets.
    /// </summary>
    public interface IGetGroupedDaily
    {
        public Task<List<GroupedDailyResult>> GetListAsync(string marketDate, HttpClient httpClient);
    }
}
