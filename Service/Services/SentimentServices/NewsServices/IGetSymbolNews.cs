using Service.Services.SentimentServices.NewsServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services.SentimentServices.NewsServices
{
    public interface IGetSymbolNews
    {
        Task<List<NewsResult>> GetListAsync(string ticker, string greaterThanOrEqualPublishedDate);
    }
}
