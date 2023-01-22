using System;
using System.Collections.Generic;
using System.Net.Http;
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
        private readonly IGetGroupedDaily _getGroupedDaily;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISeedSymbolsCommand _seedSymbolsCommand;
        private readonly IProcessPostMarketBiggestMoversCommand _processPostMarketBiggestMoversCommand;

        private readonly List<Resources.UserInputCommand> userInputCommands = Resources.GetUserInputCommands();

        public Worker(ILogger<Worker> logger
            , IGetOpenClose getOpenClose
            , IGetSymbolNews getSymbolNews
            , IGetStockTicker getStockTicker
            , IGetGroupedDaily getGroupedDaily
            , IHttpClientFactory httpClientFactory
            , ISeedSymbolsCommand seedSymbolsCommand
            , IProcessPostMarketBiggestMoversCommand processPostMarketBiggestMoversCommand)
        {
            _logger = logger;
            _getOpenClose = getOpenClose;
            _getSymbolNews = getSymbolNews;
            _getStockTicker = getStockTicker;
            _getGroupedDaily = getGroupedDaily;
            _httpClientFactory = httpClientFactory;
            _seedSymbolsCommand = seedSymbolsCommand;
            _processPostMarketBiggestMoversCommand = processPostMarketBiggestMoversCommand;
        }

        private void PromptUserMainMenu()
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
                PromptUserMainMenu();

                var userCommand = Console.ReadLine();

                // Populate local db with all known stock tickers - run this one time before any other commands.
                if (string.Equals(userCommand, Resources.SEED_LOCAL_DB_WITH_TICKERS.CommandId.ToString(), StringComparison.OrdinalIgnoreCase)
                    || string.Equals(userCommand, Resources.SEED_LOCAL_DB_WITH_TICKERS.CommandName, StringComparison.OrdinalIgnoreCase))
                {
                    var allTickers = await _getStockTicker.GetAllAsync();
                    await _seedSymbolsCommand.ExecuteAsync(allTickers);

                    Console.WriteLine($"Success - there are {allTickers.Count} supported tickers.");
                }

                if (string.Equals(userCommand, Resources.GET_OPEN_CLOSE_FOR_SYMBOL.CommandId.ToString(), StringComparison.OrdinalIgnoreCase)
                    || string.Equals(userCommand, Resources.GET_OPEN_CLOSE_FOR_SYMBOL.CommandName, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Enter a symbol:");
                    var userEnteredSymbol = Console.ReadLine().ToUpper();

                    Console.WriteLine("Enter a market date (yyyy-MM-dd):");
                    var userEnteredMarketDate = Console.ReadLine();

                    var httpClient = _httpClientFactory.CreateClient();
                    var openClose = await _getOpenClose.GetAsync(userEnteredSymbol, userEnteredMarketDate, httpClient);

                    Console.WriteLine($"Open/Close data for GME on {userEnteredMarketDate}:");
                    Console.WriteLine($"{JsonSerializer.Serialize(openClose)}");
                }

                if (string.Equals(userCommand, Resources.GET_POST_MARKET_BIGGEST_MOVERS.CommandId.ToString(), StringComparison.OrdinalIgnoreCase)
                    || string.Equals(userCommand, Resources.GET_POST_MARKET_BIGGEST_MOVERS.CommandName, StringComparison.OrdinalIgnoreCase))
                {
                    var utcStartTime = DateTime.UtcNow;

                    Console.WriteLine("Enter a market date (yyyy-MM-dd):");
                    var userEnteredMarketDate = Console.ReadLine();
                    await _processPostMarketBiggestMoversCommand.ExecuteAsync(userEnteredMarketDate);

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
