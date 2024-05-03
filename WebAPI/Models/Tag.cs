using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[Table("tags")]
public partial class Tag
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("tag")]
    public string? TagValue { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Tags")]
    public virtual Product? Product { get; set; }
}
