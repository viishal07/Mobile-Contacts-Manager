using System.ComponentModel.DataAnnotations;

namespace ContactManager.API.Models;

public class Contact
{
    public int Id { get; set; } 
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? LastName { get; set; }

    [MaxLength(200)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(200)]
    public string? Company { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    public bool Favorite { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
