using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VkTechTest.Models.Enums;

namespace VkTechTest.Database.Models;

[Table("user_state")]
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