using DataAccess;
using DataAccess.Enums;
using Service.Services.SymbolServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service.Services.SymbolServices
{
    public class SeedSymbolsCommand : ISeedSymbolsCommand
    {
        private readonly TradesContext _tradesContext;

        public SeedSymbolsCommand(TradesContext tradesContext)
        {
            _tradesContext = tradesContext;
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(List<StockTickerResult> stockTickerResults)
        {
            var symbols = new List<Symbol>();

            var isAnyExistingSymbols = await _tradesContext.Symbol.FirstOrDefaultAsync();
            if (isAnyExistingSymbols != null)
            {
                // except for now, there's no duplicate or update logic in place
                throw new System.Exception("Symbol data already exists, purge your local data in order to seed again.");
            }

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

    /// <summary>
    /// Creates and saves a <see cref="Symbol"/> record for each <see cref="StockTickerResult"/>.
    /// TODO: There's no update logic and no check before saving, so running this multiple times will result in duplicates.
    /// This would probably be something that ran weekly, or optionally manually so that new or updated ticker information is captured.
    /// At the moment just delete your local data and re-run the seed command to get updated info
    /// </summary>
    /// <param name="stockTickerResults"></param>
    /// <returns></returns>
    public interface ISeedSymbolsCommand
    {
        Task ExecuteAsync(List<StockTickerResult> stockTickerResults);
    }
}
