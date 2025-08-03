namespace Gudena.Api.DTOs;

public class CreditCardDto
{
    public string CardNumber { get; set; }
    public string CvvCode { get; set; }
    public string ExpiryDate { get; set; }
    public string HolderFullName { get; set; }
}