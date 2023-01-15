using DataAccess;
using Service.Services.SentimentServices.NewsServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services.ExtendedHoursServices
{
    public class ProcessPostMarketBiggestMoversCommand : IProcessPostMarketBiggestMoversCommand
    {
        private readonly TradesContext _tradesContext;
        private readonly IGetSymbolNews _getSymbolNews;
        private readonly IGetPostMarketBiggestMovers _getPostMarketBiggestMovers;

        public ProcessPostMarketBiggestMoversCommand(TradesContext tradesContext
            , IGetSymbolNews getSymbolNews
            , IGetPostMarketBiggestMovers getPostMarketBiggestMovers)
        {
            _tradesContext = tradesContext;
            _getSymbolNews = getSymbolNews;
            _getPostMarketBiggestMovers = getPostMarketBiggestMovers;
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(string openCloseDate)
        {
            var marketDate = DateTime.Parse(openCloseDate).Date;
            var biggestMovers = await _getPostMarketBiggestMovers.GetListAsync(openCloseDate);

            var extendedHoursBiggestMovers = new List<ExtendedHoursBiggestMovers>();
            foreach (var biggestMover in biggestMovers)
            {
                extendedHoursBiggestMovers.Add(
                new ExtendedHoursBiggestMovers
                {
                    CreatedDateTimeUtc = DateTime.UtcNow,
                    MarketDate = marketDate,
                    PriceAfterHours = biggestMover.PriceAfterHours,
                    PriceClose = biggestMover.PriceClose,
                    PriceOpen = biggestMover.PriceOpen,
                    SymbolId = biggestMover.SymbolId
                });
            };
            await _tradesContext.ExtendedHoursBiggestMovers.AddRangeAsync(extendedHoursBiggestMovers);
            await _tradesContext.SaveChangesAsync();

            var symbolNews = new List<SymbolNews>();
            foreach (var biggestMover in extendedHoursBiggestMovers)
            {
                var newsResults = await _getSymbolNews.GetListAsync(biggestMover.Symbol.Symbol1, openCloseDate);
                foreach (var newsResult in newsResults)
                {
                    symbolNews.Add(new SymbolNews
                    {
                        SymbolId = biggestMover.SymbolId,
                        Author = newsResult.Author,
                        CreatedDateTime = DateTime.UtcNow,
                        PublishedDateTime = newsResult.PublishedDateTime,
                        PublisherName = newsResult.PublisherName,
                        PublisherUrl = newsResult.PublisherUrl,
                        RelevantSymbolsCsv = string.Join(",", newsResult.RelevantSymbols),
                        Summary = newsResult.Summary,
                        Title = newsResult.Title,
                        Url = newsResult.Url
                    });
                }
            }

            await _tradesContext.SymbolNews.AddRangeAsync(symbolNews);
            await _tradesContext.SaveChangesAsync();
        }
    }

    public interface IProcessPostMarketBiggestMoversCommand
    {
        /// <summary>
        /// This command does the following:
        ///     1.) Find symbols that have increased/decreased a significant amount (configurable) after hours.
        ///     2.) Saves results
        ///     3.) Gets news for each symbol result
        ///     4.) Saves news results
        /// </summary>
        /// <param name="openCloseDate"></param>
        /// <returns></returns>
        Task ExecuteAsync(string openCloseDate);
    }
}
