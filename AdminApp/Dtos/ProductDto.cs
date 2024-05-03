using AdminApp.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminApp.Dtos;

public class ProductDto
{
    public int Id { get; set; }

    public string? Name { get; set; } = "";

    public string? ShortDescription { get; set; } = "";

    public string? Description { get; set; } = "";

    public double? Price { get; set; } = 0;

    public DateTime? CreatedAt { get; set; }

    public string? ProductSource { get; set; }

    public List<string> Tags { get; set; } = new();

    public List<int> CategoryIds { get; set; } = new();

    public int Stt {get;set;}

    public string? CreatedAtString => CreatedAt.NulltableDateTime2Str();

    public string? TagsString => string.Join(", ", Tags);

    public string? PriceString => Price != null ? ((double)Price).ToString("#,##0") : null;
}
