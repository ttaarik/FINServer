namespace FINServer.Models
{
    public class Card
    {
        public int CardId { get; set; }
        public int CustomerId { get; set; }
        public int CardNumber { get; set; }
        public CardType card_type { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int CVV { get; set; }
        public int PIN { get; set; }
        public decimal MonthlySpending { get; set; }
        public decimal CredLimit { get; set; }
        public int Active { get; set; }
    }


    public enum CardType
    {
        Visa,
        MasterCard,
    }
}
