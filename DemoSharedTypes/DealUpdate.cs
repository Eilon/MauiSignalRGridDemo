namespace DemoSharedTypes
{
    /// <summary>
    /// Defines a deal update message that is used to send deal updates to clients.
    /// </summary>
    public class DealUpdate
    {
        public string? SmartID { get; set; }
        public decimal? NewPrice { get; set; }
    }
}
