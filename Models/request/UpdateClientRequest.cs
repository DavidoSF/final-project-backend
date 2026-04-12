using System.ComponentModel.DataAnnotations;

namespace final_project_backend.Models.request;

public class UpdateClientRequest
{
    [Required]
    public Guid Id { get; set; }

    [MaxLength(200)]
    public string? FullName { get; set; }

    [MaxLength(256)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(30)]
    public string? PhoneNumber { get; set; }

    [MaxLength(200)]
    public string? CompanyName { get; set; }
}
