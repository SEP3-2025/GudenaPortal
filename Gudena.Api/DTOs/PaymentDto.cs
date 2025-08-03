namespace Gudena.Api.DTOs
{
    public class PaymentDto
    {
        public string PaymentMethod { get; set; }
        public CreditCardDto CreditCard { get; set; }
    }
}
