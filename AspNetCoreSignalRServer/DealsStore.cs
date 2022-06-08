using DemoSharedTypes;

namespace AspNetCoreSignalRServer
{
    /// <summary>
    /// Storage of all current deals. Upon creation, a pseudo-random set of deals is created.
    /// </summary>
    public class DealsStore
    {
        public DealsStore()
        {
            Deals = Enumerable
                .Range(0, 100)
                .Select(seed => new DealInfo
                {
                    SmartID = $"SID-{seed}-XYZ-{seed * 984}",
                    TradeDate = DateTimeOffset.Now.AddDays(seed * -5),
                    Trader = $"Smart trader {seed}",
                    ProductType = (seed % 3) switch { 0 => "Fruit", 1 => "Toys", 2 => "Houses", _ => "Unknown" },
                    TransactionType = (seed % 3) == 0 ? DealTransactionType.Purchase : DealTransactionType.Sale,
                    Price = 2000m + (seed - 50) * 13,
                })
                .ToList();
        }

        public IList<DealInfo> Deals { get; }
    }
}
