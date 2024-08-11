namespace FINServer.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int AccountID { get; set; }
        public decimal Amount { get; set; }
        public TransActionType TransactionType { get; set; }
        public int TargetAccountId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Description { get; set; }
    }

    public enum TransActionType
    {
        Withdrawal,
        Deposit,
        Transfer
    }
}
