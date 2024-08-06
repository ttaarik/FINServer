namespace FINServer.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public int CustomerId { get; set; }
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
    }

    public enum AccountType
    {
        Girokonto,
        Sparbuch,
        Tagesgeldkonto
    }
}
