﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using VkTechTest.Models.Enums;

namespace VkTechTest.Database.Models;

[Table("user_groups")]
[Index(nameof(Code), IsUnique = true)]
public class UserGroupEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    [Key]
    public long Id { get; set; }
    
    [Column("code")]
    public UserGroupType Code { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
}