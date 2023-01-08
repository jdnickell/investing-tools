using DataAccess;
using DataAccess.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services.SymbolServices
{
    public class GetSymbols : IGetSymbols
    {
        private readonly TradesContext _tradesContext;

        public GetSymbols(TradesContext tradesContext)
        {
            _tradesContext = tradesContext;
        }

        public async Task<Symbol> GetAsync(string symbol)
        {
            var result = await _tradesContext.Symbol.FirstOrDefaultAsync(x => x.Symbol1 == symbol);
            return result;
        }

        public async Task<List<Symbol>> GetListAsync()
        {
            var symbols = await _tradesContext.Symbol.Where(x => x.SymbolTypeId == (int)SymbolType.Stock).ToListAsync();
            return symbols;
        }
    }

    public interface IGetSymbols
    {
        Task<Symbol> GetAsync(string symbol);
        Task<List<Symbol>> GetListAsync();
    }
}
