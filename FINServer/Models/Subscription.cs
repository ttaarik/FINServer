namespace FINServer.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public int CustomerId { get; set; }
        public string ServiceName { get; set; }
        public decimal MonthlyFee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Active { get; set; }
    }
}
