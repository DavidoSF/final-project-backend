using System.ComponentModel.DataAnnotations;

namespace final_project_backend.Models.request;

public class UpdateServiceRequest
{
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99.")]
    public decimal? Price { get; set; }

    [Range(1, 43200, ErrorMessage = "Duration must be between 1 and 43200 minutes.")]
    public int? DurationInMinutes { get; set; }
}
