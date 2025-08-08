namespace Gudena.Api.DTOs
{
    public record ShippingCostPreviewDto(
        string BusinessName,
        string OriginCountry,
        string OriginPostalCode,
        string DestinationCountry,
        string DestinationPostalCode,
        decimal? Cost,
        string Message
    );
}