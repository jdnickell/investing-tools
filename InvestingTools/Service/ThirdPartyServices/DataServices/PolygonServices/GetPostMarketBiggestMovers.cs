using Service.Services.AfterHoursServices.Models;
using Service.Services.ExtendedHoursServices;
using Service.Services.SymbolServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.ThirdPartyServices.DataServices.PolygonServices
{
    public class GetPostMarketBiggestMovers : IGetPostMarketBiggestMovers
    {
        private readonly IGetSymbols _getSymbols;
        private readonly IGetOpenClose _getOpenClose;

        const int POSITIVE_GAIN_PERCENT_THRESHOLD = 10;

        public GetPostMarketBiggestMovers(IGetSymbols getSymbols
            , IGetOpenClose getOpenClose)
        {
            _getSymbols = getSymbols;
            _getOpenClose = getOpenClose;
        }

        public async Task<List<DailyOpenCloseResult>> GetListAsync(string openCloseDate)
        {
            var tickers = await _getSymbols.GetListAsync();
            var dailyOpenCloseResultAboveThreshold = new List<DailyOpenCloseResult>();

            foreach(var ticker in tickers)
            {
                try
                {
                    var openClose = await _getOpenClose.GetAsync(ticker.Symbol1, openCloseDate);
                    openClose.SymbolId = ticker.Id; //todo: this was lazy, it should be mapped from the result to another domain that then includes this ID

                    var percentChange = GetPercentChange(openClose.PriceClose, openClose.PriceAfterHours);

                    if (percentChange >= POSITIVE_GAIN_PERCENT_THRESHOLD)
                        dailyOpenCloseResultAboveThreshold.Add(openClose);
                }
                catch
                {
                    //todo logging
                }
            }

            return dailyOpenCloseResultAboveThreshold;
        }

        private decimal GetPercentChange(decimal closePrice, decimal afterHoursPrice)
        {
            var priceChange = afterHoursPrice - closePrice;
            var percentChanged = (priceChange / closePrice) * 100;
            return percentChanged;
        }
    }
}
