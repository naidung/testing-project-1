using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[Table("categories_products_mapping")]
public partial class CategoriesProductsMapping
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("CategoriesProductsMappings")]
    public virtual Category? Category { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("CategoriesProductsMappings")]
    public virtual Product? Product { get; set; }
}
