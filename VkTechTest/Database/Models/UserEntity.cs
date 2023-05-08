using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VkTechTest.Database.Models;

[Table("users")]
[Index(nameof(Login), IsUnique = true)]
public class UserEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    [Key]
    public long Id { get; set; }
    
    [Column("login")]
    public required string Login { get; set; }
    
    [Column("password")]
    public required string Password { get; set; }
    
    [Column("created_date")]
    [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedDate { get; set; }
    
    [Column("user_group_id")]
    public required long UserGroupId { get; set; }
    
    [ForeignKey(nameof(UserGroupId))]
    public UserGroupEntity UserGroup { get; set; }
    
    [Column("user_state_id")]
    public required long UserStateId { get; set; }
    
    [ForeignKey(nameof(UserStateId))]
    public UserStateEntity UserState { get; set; }
}