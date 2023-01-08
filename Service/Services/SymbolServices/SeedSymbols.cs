using DataAccess;
using DataAccess.Enums;
using Service.Services.SymbolServices.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services.SymbolServices
{
    public class SeedSymbolsCommand : ISeedSymbolsCommand
    {
        private readonly TradesContext _tradesContext;

        public SeedSymbolsCommand(TradesContext tradesContext)
        {
            _tradesContext = tradesContext;
        }

        public async Task ExecuteAsync(List<StockTickerResult> stockTickerResults)
        {
            var symbols = new List<Symbol>();

            foreach(var stockTickerResult in stockTickerResults)
            {
                symbols.Add(new Symbol
                {
                    Name = stockTickerResult.Name,
                    SymbolTypeId = (int)SymbolType.Stock,
                    Symbol1 = stockTickerResult.Symbol
                });
            }

            await _tradesContext.Symbol.AddRangeAsync(symbols);
            await _tradesContext.SaveChangesAsync();
        }
    }

    public interface ISeedSymbolsCommand
    {
        Task ExecuteAsync(List<StockTickerResult> stockTickerResults);
    }
}
