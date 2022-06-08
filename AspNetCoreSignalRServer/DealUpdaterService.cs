using DemoSharedTypes;
using Microsoft.AspNetCore.SignalR;

namespace AspNetCoreSignalRServer
{
    /// <summary>
    /// A background service that updates deals every 0.5 seconds. The changes are broadcast via
    /// SignalR to clients listening to the "UpdateDeal" message.
    /// </summary>
    public class DealUpdaterService : BackgroundService
    {
        private readonly IHubContext<DealsHub> _hubContext;
        private readonly ILogger<DealUpdaterService> _logger;
        private readonly DealsStore _dealsStore;

        public DealUpdaterService(IHubContext<DealsHub> hubContext, ILogger<DealUpdaterService> logger, DealsStore dealsStore)
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
}
