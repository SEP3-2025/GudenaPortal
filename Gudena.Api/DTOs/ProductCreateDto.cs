using System.ComponentModel.DataAnnotations;

namespace Gudena.Api.DTOs;

public class ProductCreateDto
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public string Thumbnail { get; set; }

    [Required]
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Unit { get; set; }
    public bool IsDeleted { get; set; }
    public string ReferenceUrl { get; set; }
    public bool IsReturnable { get; set; }

    [Required]
    public int CategoryId { get; set; }
}