using System.ComponentModel.DataAnnotations;

namespace VkTechTest;

public sealed class ApplicationOptions
{
    public static string SectionName = "Application";
    
    [Range(10, 10000)]
    public required int MaxPageSize { get; set; }
}