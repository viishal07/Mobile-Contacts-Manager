using System.ComponentModel.DataAnnotations;

namespace ContactManager.API.DTOs;

public class ContactDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Company { get; set; }
    public string? Address { get; set; }
    public bool Favorite { get; set; }
    public DateTime CreatedAt { get; set; }
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string Initials => GetInitials();

    private string GetInitials()
    {
        var first = string.IsNullOrEmpty(FirstName) ? "" : FirstName[0].ToString().ToUpper();
        var last = string.IsNullOrEmpty(LastName) ? "" : LastName[0].ToString().ToUpper();
        return $"{first}{last}";
    }
}

public class CreateContactDto
{
    [Required(ErrorMessage = "First name is required")]
    [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string? LastName { get; set; }

    [MaxLength(200)]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }

    [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    public string? PhoneNumber { get; set; }

    [MaxLength(200, ErrorMessage = "Company cannot exceed 200 characters")]
    public string? Company { get; set; }

    [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? Address { get; set; }

    public bool Favorite { get; set; } = false;
}

public class UpdateContactDto
{
    [Required(ErrorMessage = "First name is required")]
    [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? LastName { get; set; }

    [MaxLength(200)]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(200)]
    public string? Company { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    public bool Favorite { get; set; } = false;
}

public class ContactQueryParams
{
    public string? Search { get; set; }
    public bool? Favorite { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}
