using Gudena.Data.Entities;

namespace Gudena.Api.DTOs;

public class BasketDto
{
    public int BasketId { get; set; }
    public ICollection<BasketItemDto> BasketItems { get; set; }
}