namespace Gudena.Api.DTOs
{
    public class PaymentRequestDto
    {
        public int BasketId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}