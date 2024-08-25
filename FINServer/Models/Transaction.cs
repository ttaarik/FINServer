namespace FINServer.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int SenderAccountID { get; set; }
        public int ReceiverAccountID { get; set; }
        public decimal Amount { get; set; }
        public TransActionType TransactionType { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Description { get; set; }
    }

    public enum TransActionType
    {
        Withdrawal,
        Deposit,
        Transfer,
        Gehalt
    }
}
