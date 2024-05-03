namespace AdminApp.Dtos;

public class ProductQueryDto
{
    public List<ProductDto> Products { get; set; } = new();
    public int TotalPages { get; set; }
}
