namespace Gudena.Api.DTOs
{
    public class PaymentRequestDto
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}