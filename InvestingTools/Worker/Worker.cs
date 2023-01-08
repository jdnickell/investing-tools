using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Services.ExtendedHoursServices;
using Service.Services.SentimentServices.NewsServices;
using Service.Services.SymbolServices;

namespace Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IGetOpenClose _getOpenClose;
        private readonly IGetSymbolNews _getSymbolNews;
        private readonly IGetStockTicker _getStockTicker;
        private readonly ISeedSymbolsCommand _seedSymbolsCommand;
        private readonly IGetPostMarketBiggestMovers _getPostMarketBiggestMovers;
        private readonly IProcessPostMarketBiggestMoversCommand _processPostMarketBiggestMoversCommand;

        private readonly List<Resources.UserInputCommand> userInputCommands = Resources.GetUserInputCommands();

        public Worker(ILogger<Worker> logger
            , IGetOpenClose getOpenClose
            , IGetSymbolNews getSymbolNews
            , IGetStockTicker getStockTicker
            , ISeedSymbolsCommand seedSymbolsCommand
            , IGetPostMarketBiggestMovers getPostMarketBiggestMovers
            , IProcessPostMarketBiggestMoversCommand processPostMarketBiggestMoversCommand)
        {
            _logger = logger;
            _getOpenClose = getOpenClose;
            _getSymbolNews = getSymbolNews;
            _getStockTicker = getStockTicker;
            _seedSymbolsCommand = seedSymbolsCommand;
            _getPostMarketBiggestMovers = getPostMarketBiggestMovers;
            _processPostMarketBiggestMoversCommand = processPostMarketBiggestMoversCommand;
        }

        private void PromptUser()
        {
            //Someone make a UI plz
            Console.WriteLine("What do you want to do?");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Options:");
            foreach (var inputUserCommand in userInputCommands)
            {
                Console.WriteLine($"{inputUserCommand.CommandId} or {inputUserCommand.CommandName}");
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                PromptUser();

                var userCommand = Console.ReadLine();

                if (string.Equals(userCommand, Resources.GET_ALL_STOCK_TICKERS.CommandId.ToString(), StringComparison.OrdinalIgnoreCase)
                    || string.Equals(userCommand, Resources.GET_ALL_STOCK_TICKERS.CommandName, StringComparison.OrdinalIgnoreCase))
                {
                    var allTickers = await _getStockTicker.GetAllAsync();

                    //uncomment to seed your database with the result - it's dumb and will insert duplicates so delete the records manually first
                    //await _seedSymbolsCommand.ExecuteAsync(allTickers);

                    Console.WriteLine($"Success - there are {allTickers.Count} supported tickers.");
                }

                if (string.Equals(userCommand, Resources.GET_OPEN_CLOSE_FOR_TEST_SYMBOL_GME.CommandId.ToString(), StringComparison.OrdinalIgnoreCase)
                    || string.Equals(userCommand, Resources.GET_OPEN_CLOSE_FOR_TEST_SYMBOL_GME.CommandName, StringComparison.OrdinalIgnoreCase))
                {
                    var testGMEDate = "2021-06-04";
                    var gmeOpenClose = await _getOpenClose.GetAsync("GME", testGMEDate);

                    Console.WriteLine($"Open/Close data for GME on {testGMEDate}: {JsonSerializer.Serialize(gmeOpenClose)}");
                }

                if (string.Equals(userCommand, Resources.GET_POST_MARKET_BIGGEST_MOVERS.CommandId.ToString(), StringComparison.OrdinalIgnoreCase)
                    || string.Equals(userCommand, Resources.GET_POST_MARKET_BIGGEST_MOVERS.CommandName, StringComparison.OrdinalIgnoreCase))
                {
                    //check for weekend or holiday 
                    var utcStartTime = DateTime.UtcNow;
                    var timeUtc = utcStartTime.AddDays(-4);
                    TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                    var testOpenCloseDate = easternTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                    await _processPostMarketBiggestMoversCommand.ExecuteAsync(testOpenCloseDate);

                    //var postMarketBiggestMovers = await _getPostMarketBiggestMovers.GetListAsync(testOpenCloseDate);

                    //Console.WriteLine($"The following tickers gained more than 10% after hours on {testOpenCloseDate}");

                    //foreach (var bigMover in postMarketBiggestMovers)
                    //{
                    //    var percentChange = ((bigMover.PriceAfterHours - bigMover.PriceClose) / bigMover.PriceClose) * 100;
                    //    Console.WriteLine($"Symbol: {bigMover.Symbol} Percent Change: {percentChange:F} After Hours: {bigMover.PriceAfterHours} Close: {bigMover.PriceClose} Open: {bigMover.PriceOpen}");
                    //}

                    //Console.WriteLine("Getting news results for each symbol...");
                    //foreach (var bigMover in postMarketBiggestMovers)
                    //{
                    //    var newsResults = await _getSymbolNews.GetListAsync(bigMover.Symbol, testOpenCloseDate);

                    //    Console.WriteLine(bigMover.Symbol);
                    //    foreach (var newsResult in newsResults)
                    //    {
                    //        Console.WriteLine($"Published by {newsResult.PublisherName} at {newsResult.PublishedDateTime} Title: {newsResult.Title}, {newsResult.Summary}");
                    //    }
                    //}

                    Console.WriteLine($"Started: {utcStartTime}. Finished: {DateTime.Now}");
                }

                if (string.Equals(userCommand, Resources.GET_NEWS_FOR_TEST_SYMBOL_GME.CommandId.ToString(), StringComparison.OrdinalIgnoreCase)
                    || string.Equals(userCommand, Resources.GET_NEWS_FOR_TEST_SYMBOL_GME.CommandName, StringComparison.OrdinalIgnoreCase))
                {
                    //todo: add another logical relationship that associates a collection of these to a 'run' - if we run this multiple times you can group by the task id or the task completion datetime etc.
                    var newsResults = await _getSymbolNews.GetListAsync("GME", "2021-06-09");
                    foreach (var newsResult in newsResults)
                    {
                        Console.WriteLine($"Published by {newsResult.PublisherName} at {newsResult.PublishedDateTime} Title: {newsResult.Title}, {newsResult.Summary}");
                    }
                }
                
                Console.WriteLine(Environment.NewLine);
            }
        }
    }
}
