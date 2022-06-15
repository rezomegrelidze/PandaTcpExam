using System;
using PandaTcpExam.Common;

namespace PandaTcpExam;

public class QuoteDataViewModel
{
    public static QuoteDataViewModel FromQuoteData(QuoteData data)
    {
        return new QuoteDataViewModel
        {
            AssetName = data.Symbol,
            Bid = data.Bid,
            Ask = data.Ask,
            Time = new DateTime(data.TickTime).ToString("dd/MM/yyyy-HH:mm:ss.fff")
        };
    }

    public string Time { get; set; }

    public decimal Ask { get; set; }

    public decimal Bid { get; set; }

    public string AssetName { get; set; }
}