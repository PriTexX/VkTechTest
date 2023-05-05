using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VkTechTest.Database.Models;

[Table("user")]
[Index(nameof(Login), IsUnique = true)]
public class UserEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    [Key]
    public long Id { get; set; }
    
    [Column("login")]
    public string Login { get; set; }
    
    [Column("password")]
    public string Password { get; set; }
    
    [Column("created_date", TypeName = "date")]
    public DateTime CreatedDate { get; set; }
    
    [Column("user_group_id")]
    public long UserGroupId { get; set; }
    
    [Column("user_state_id")]
    public long UserStateId { get; set; } 
}