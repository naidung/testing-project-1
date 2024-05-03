using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[Table("products")]
public partial class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; } = "";

    [Column("image")]
    public string? Image { get; set; }

    [Column("short_description")]
    public string? ShortDescription { get; set; } = "";

    [Column("description")]
    public string? Description { get; set; } = "";

    [Column("created_by_id")]
    public int? CreatedById { get; set; }

    [Column("price")]
    public double? Price { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; } = false;

    [Column("modified_at")]
    public DateTime? ModifiedAt { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<CategoriesProductsMapping> CategoriesProductsMappings { get; set; } = new List<CategoriesProductsMapping>();

    [ForeignKey("CreatedById")]
    [InverseProperty("Products")]
    public virtual User? CreatedBy { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}