# Introduction

1/9/2023 - migrating private repo from 2021 and updating package and .net framework versions.

This works locally if you figure out the steps to create your local database, then create your own Polygon api key. You would need the pro version in order to get more than 5 results, but you can hard code some test data to try it out.

We apparently didn't document or add tests for anything 🧠

1/9/23 - To get started:

- Create a local sql database called InvestingTools
- Run the script in DataAcces / SQL / CreateAndSeed.sql
- Edit Worker / appsettings.Development.json
  - Update your connection string, create your free Api Polygon api key - probably don't check this in, but if you do by accident just deactivate it and make a new one.
- uncomment `await _seedSymbolsCommand.ExecuteAsync(allTickers);` in Worker.cs
  - You want this to run once just to seed initial data as this is obviously some temporary hacked in code to get started, so comment it back out after running once.
  - You only get 5 requests per minute, so there's a counter in GetStockTicker.cs - you can figure out a timer to eventually populate these or sign up for a paid account and remove this count logic. This still captures thousands of symbols so it's enough to play with.
- Run the project, choose 1 - this will populate your Symbol table with current stock ticker symbols.

From here you can try the other commands and debug figure out what each command does. Notice the hard coded date in Worker.cs.

1/15/23 - Example strategy use case

- At 8pm central time, run (3) GET_POST_MARKET_BIGGEST_MOVERS
- Review results, looking for relevant positive news that contributed to rise.
- The user manually chooses any to continue with the following day.
- The next day, 10-15 minutes before open, capture any that have cooled down - meaning they increased by < 10% during pre-market
- Buy at open
- Plan to sell within the first hour, take any reasonable gain (%?) or sell after a 10% decline
  - Also watch SPY, and sell if there's a 10% decline
