namespace Gudena.Api.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public int OrderId { get; set; }
    }
}
