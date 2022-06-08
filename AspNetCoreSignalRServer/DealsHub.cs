using DemoSharedTypes;
using Microsoft.AspNetCore.SignalR;

namespace AspNetCoreSignalRServer
{
    /// <summary>
    /// SignalR Hub for clients to get a list of all deals.
    /// </summary>
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
    }
}
