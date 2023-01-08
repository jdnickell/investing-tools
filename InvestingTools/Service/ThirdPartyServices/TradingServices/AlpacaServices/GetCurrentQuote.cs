using Alpaca.Markets;
using System.Threading.Tasks;

namespace Service.ThirdPartyServices.TradingServices.AlpacaServices
{
    public class GetCurrentQuote : IGetCurrentQuote
    {
        private IGetAlpacaClient _getAlpacaClient;

        public GetCurrentQuote(IGetAlpacaClient getAlpacaClient)
        {
            _getAlpacaClient = getAlpacaClient;
        }
        public Task<ILastQuote> GetAsync(string ticker)
        {
            var client = _getAlpacaClient.GetDataClient();
            var quote = client.GetLastQuoteAsync(ticker);
            return quote;
        }
    }

    public interface IGetCurrentQuote
    {
        Task<ILastQuote> GetAsync(string ticker);        
    }
}
