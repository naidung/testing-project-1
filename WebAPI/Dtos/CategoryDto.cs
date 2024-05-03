using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos;

public class CategoryDto
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ParentId { get; set; }

    public string? ParentName { get; set; }
}
