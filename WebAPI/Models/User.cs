using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[Table("users")]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("full_name")]
    public string? FullName { get; set; } = "";

    [Column("phone")]
    public string? Phone { get; set; } = "";

    [Column("email")]
    public string? Email { get; set; } = "";

    [Column("user_name")]
    public string? UserName { get; set; } = "";

    [Column("password")]
    public string? Password { get; set; } = "";

    [Column("access_token")]
    public string? AccessToken { get; set; } = "";

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("modified_at")]
    public DateTime? ModifiedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; } = false;

    [InverseProperty("CreatedBy")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    [InverseProperty("CreatedBy")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [InverseProperty("User")]
    public virtual ICollection<SystemLog> SystemLogs { get; set; } = new List<SystemLog>();

    [InverseProperty("User")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
