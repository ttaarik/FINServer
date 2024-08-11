namespace FINServer.Models
{
    public class SupportTicket
    {
        public int TicketId { get; set; }
        public int CustomerId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt{ get; set; }
    }

    public enum TicketStatus
    {
        Resolved,
        InProgress,
        Open
    }
}
