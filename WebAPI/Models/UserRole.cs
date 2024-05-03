using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[Table("user_roles")]
public partial class UserRole
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("role_id")]
    public int? RoleId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; } = false;

    [ForeignKey("UserId")]
    [InverseProperty("UserRoles")]
    public virtual User? User { get; set; }
}
