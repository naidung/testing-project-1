using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[Table("categories")]
public partial class Category
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Column("created_by_id")]
    public int? CreatedById { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("modified_at")]
    public DateTime? ModifiedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; } = false;

    [Column("parent_id")]
    public int? ParentId { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<CategoriesProductsMapping> CategoriesProductsMappings { get; set; } = new List<CategoriesProductsMapping>();

    [ForeignKey("CreatedById")]
    [InverseProperty("Categories")]
    public virtual User? CreatedBy { get; set; }

    [ForeignKey("ParentId")]
    //[InverseProperty("InverseParent")]
    public virtual Category? Parent { get; set; }
}