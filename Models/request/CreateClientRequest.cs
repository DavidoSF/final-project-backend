using System.ComponentModel.DataAnnotations;

namespace final_project_backend.Models.request;

public class CreateClientRequest
{
    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(200)]
    public string CompanyName { get; set; } = string.Empty;
}
