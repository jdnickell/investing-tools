using Service.Services.SymbolServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services.SymbolServices
{
    public interface IGetStockTicker
    {
        /// <summary>
        /// Gets all supported StockTickerResults from the third party data service
        /// </summary>
        /// <returns></returns>
        Task<List<StockTickerResult>> GetAllAsync();
        Task<StockTickerResult> GetAsync(string symbol);
    }
}
