using Service.Services.AfterHoursServices.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.Services.ExtendedHoursServices
{
    public interface IGetOpenClose
    {
        /// <summary>
        /// Get the open, close and afterhours prices of a stock symbol on a certain date. 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="openCloseDate"></param>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        Task<DailyOpenCloseResult> GetAsync(string symbol, string openCloseDate, HttpClient httpClient);
    }
}
