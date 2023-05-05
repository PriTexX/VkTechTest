using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using VkTechTest.Models.Enums;

namespace VkTechTest.Database.Models;

[Table("user_state")]
[Index(nameof(Code), IsUnique = true)]
public class UserStateEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    [Key]
    public long Id { get; set; }
    
    [Column("code")]
    public UserStateType Code { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
}