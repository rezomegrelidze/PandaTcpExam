using System.Text;
using PandaTcpExam.Common;

namespace PandaTcpExam.Server;

public  class RandomDataGenerator
{
    private List<QuoteData> Data = new();

    public RandomDataGenerator()
    {
        var possibleSymbols = new[]
        {
            "GOOG", "APPL", "MSFT", "AMZN",
        }.Concat(GenerateRandomSymbols(20)).ToList();

        InitializeData(possibleSymbols);
    }

    private IEnumerable<string> GenerateRandomSymbols(int count,int symbolLength = 4)
    {
        for (int i = 0; i < count; i++)
        {
            var rand = new Random();

            StringBuilder symbol = new StringBuilder();

            for (int j = 0; j < symbolLength; j++)
            {
                symbol.Append((char)('A' + rand.Next(26)));
            }
            yield return symbol.ToString();
        }
    }

    private void InitializeData(List<string> possibleSymbols)
    {
        for (int i = 0; i < 200; i++)
        {
            possibleSymbols.Shuffle();
            foreach (var possibleSymbol in possibleSymbols)
            {
                var quoteData = new QuoteData()
                {
                    Symbol = possibleSymbol,
                    Ask = GetRandomDecimal(),
                    Bid = GetRandomDecimal(),
                };

                Data.Add(quoteData);
            }
        }
    }



    private decimal GetRandomDecimal()
    {
        var rand = new Random();
        return decimal.Parse($"{rand.Next(100000)}.{rand.Next(10000)}");
    }

    public IEnumerable<QuoteData> GetData()
    {
        return Data;
    }
}