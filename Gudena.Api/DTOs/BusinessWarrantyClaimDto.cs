namespace Gudena.Api.DTOs;

public class BusinessWarrantyClaimDto
{
    public int Id { get; set; }
    public int OrderItemId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; }
}

public class UpdateBusinessWarrantyClaimStatusDto
{
    public string Status { get; set; }
}