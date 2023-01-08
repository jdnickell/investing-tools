using Alpaca.Markets;

namespace Service.ThirdPartyServices.TradingServices.AlpacaServices
{
    public class GetAlpacaClient : IGetAlpacaClient
    {        
        private static readonly string KEY_ID = "PKJ8LTJPMV3RM8HQMF0V";
        private static readonly string SECRET_KEY = "mNSsGMHnTy6qRyJu3Uqz8O08X64cLgkPtnONOt0o";

        private IAlpacaDataClient alpacaDataClient = Environments.Paper.GetAlpacaDataClient(new SecretKey(KEY_ID, SECRET_KEY));
        private IAlpacaTradingClient alpacaTradingClient = Environments.Paper.GetAlpacaTradingClient(new SecretKey(KEY_ID, SECRET_KEY));

        public IAlpacaDataClient GetDataClient()
        {
            return alpacaDataClient;
        }

        public IAlpacaTradingClient GetTradingClient()
        {
            return alpacaTradingClient;
        }
    }
    public interface IGetAlpacaClient
    {
        IAlpacaDataClient GetDataClient();
        IAlpacaTradingClient GetTradingClient();
    }
}
