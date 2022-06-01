using DemoSharedTypes;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.IO.Pipelines;

namespace AspNetCoreSignalRServer
{
    public class DealUpdater : BackgroundService
    {
        private readonly IHubContext<DealsHub> _hubContext;
        private readonly ILogger<DealUpdater> _logger;
        private readonly DealsStore _dealsStore;

        public DealUpdater(IHubContext<DealsHub> hubContext, ILogger<DealUpdater> logger, DealsStore dealsStore)
        {
            _hubContext = hubContext;
            _logger = logger;
            _dealsStore = dealsStore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var dealToUpdate = _dealsStore.Deals[Random.Shared.Next(_dealsStore.Deals.Count)];
                var dealUpdate = new DealUpdate
                {
                    SmartID = dealToUpdate.SmartID,
                    NewPrice = dealToUpdate.Price += Random.Shared.Next(-500, 500),
                };
                _logger.LogInformation("Updated deal: {DEAL_SMART_ID} to price {NEW_PRICE}", dealUpdate.SmartID, dealUpdate.NewPrice);

                await _hubContext.Clients.All.SendAsync("UpdateDeal", dealUpdate, cancellationToken: stoppingToken);

                await Task.Delay(500, stoppingToken);
            }
        }
    }

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

    public class DealsHub : Hub
    {
        private readonly DealsStore _dealsStore;

        public DealsHub(DealsStore dealsStore)
        {
            _dealsStore = dealsStore;
        }

        public IList<DealInfo> GetDeals()
        {
            return _dealsStore.Deals;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
