namespace PandaTcpExam.Common
{
    public class QuoteData
    {
        public string Symbol { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public long TickTime { get; set; }
    }
}