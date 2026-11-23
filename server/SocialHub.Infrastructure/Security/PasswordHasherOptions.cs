using System.ComponentModel.DataAnnotations;

namespace SocialHub.Infrastructure.Security;

public class PasswordHasherOptions
{
    public const string SectionName = "PasswordHasher";

    [Range(4, 31, ErrorMessage = "WorkFactor must be between 4 and 31.")]
    public int WorkFactor { get; set; } = 12;

}