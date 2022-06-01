namespace DemoSharedTypes
{
    public class DealInfo
    {
        public string? SmartID { get; set; }
        public DateTimeOffset TradeDate { get; set; }
        public string? Trader { get; set; }
        public string? ProductType { get; set; }
        public DealTransactionType? TransactionType { get; set; }
        public decimal? Price { get; set; }
    }
}
