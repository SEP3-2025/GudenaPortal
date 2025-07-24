using System.ComponentModel.DataAnnotations;

namespace Gudena.Api.DTOs;

public class ProductCreateDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    public string Thumbnail { get; set; }

    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Stock { get; set; }
    [Required]
    public string Unit { get; set; }
    public bool IsDeleted { get; set; }
    [Required]
    public string ReferenceUrl { get; set; }
    [Required]
    public bool IsReturnable { get; set; }

    [Required]
    public int CategoryId { get; set; }
    
    public List<string> MediaUrls { get; set; } = new();
}