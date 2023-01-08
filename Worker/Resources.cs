using System.Collections.Generic;

namespace Worker
{
    public static class Resources
    {
        public static UserInputCommand GET_ALL_STOCK_TICKERS = new UserInputCommand { CommandId = 1, CommandName="GetAllTickers" };
        public static UserInputCommand GET_OPEN_CLOSE_FOR_TEST_SYMBOL_GME = new UserInputCommand { CommandId = 2, CommandName = "GetOpenCloseForTestSymbol" };
        public static UserInputCommand GET_POST_MARKET_BIGGEST_MOVERS = new UserInputCommand { CommandId = 3, CommandName = "GetPostMarketBiggestMovers" };
        public static UserInputCommand GET_NEWS_FOR_TEST_SYMBOL_GME = new UserInputCommand { CommandId = 4, CommandName = "GetSymbolNews" };

        public static List<UserInputCommand> UserInputCommands = new List<UserInputCommand>();

        public static List<UserInputCommand> GetUserInputCommands()
        {
            UserInputCommands.Add(GET_ALL_STOCK_TICKERS);
            UserInputCommands.Add(GET_OPEN_CLOSE_FOR_TEST_SYMBOL_GME);
            UserInputCommands.Add(GET_POST_MARKET_BIGGEST_MOVERS);
            UserInputCommands.Add(GET_NEWS_FOR_TEST_SYMBOL_GME);

            return UserInputCommands;
        }

        public class UserInputCommand
        {
            public int CommandId { get; set; }
            public string CommandName { get; set; }
        }
    }
}
