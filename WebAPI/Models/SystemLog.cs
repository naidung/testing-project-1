using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[Table("system_logs")]
public partial class SystemLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("new_data")]
    public string? NewData { get; set; }

    [Column("old_data")]
    public string? OldData { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("SystemLogs")]
    public virtual User? User { get; set; }
}
