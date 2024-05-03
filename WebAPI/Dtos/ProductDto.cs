using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Dtos;

public class ProductDto
{
    public int Id { get; set; }

    public string? Name { get; set; } = "";

    public string? ShortDescription { get; set; } = "";

    public string? Description { get; set; } = "";

    public double? Price { get; set; } = 0;

    public DateTime? CreatedAt { get; set; }

    public string? ProductSource { get; set; }

    public List<int> CategoryIds { get; set; } = new();

    public List<string> Tags { get; set; } = new();
}
