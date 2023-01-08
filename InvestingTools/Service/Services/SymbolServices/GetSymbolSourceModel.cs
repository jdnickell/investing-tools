using DataAccess;
using Microsoft.EntityFrameworkCore;
using Service.Services.SymbolServices.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services.SymbolServices
{
    public class GetSymbolSourceModel : IGetSymbolSourceModel
    {
        private readonly TradesContext _tradesContext;

        public GetSymbolSourceModel(TradesContext tradesContext)
        {
            _tradesContext = tradesContext;
        }

        public async Task<List<SymbolSourceModel>> GetListAsync()
        {
            var symbolSourceModels = new List<SymbolSourceModel>();

            var symbolList = await _tradesContext.SymbolSource
                .Include(x => x.Symbol)
                .Include(x => x.Source)
                .Where(x => !x.IsDeleted && !x.Source.IsDeleted)
                .ToListAsync();

            foreach(var symbol in symbolList)
            {
                symbolSourceModels.Add(new SymbolSourceModel
                {
                    IsSandbox = symbol.Source.IsSandbox,
                    SourceApiKey = symbol.Source.ApiKey,
                    SourceApiUrl = symbol.Source.Url,
                    SourceName = symbol.Source.Name,
                    Symbol = symbol.Symbol.Symbol1,
                    SymbolName = symbol.Symbol.Name
                });
            }

            return symbolSourceModels;
        }
    }

    public interface IGetSymbolSourceModel
    {
        Task<List<SymbolSourceModel>> GetListAsync();
    }
}
