using System.ComponentModel.DataAnnotations;

namespace final_project_backend.Models.request;

public class ServiceIdRequest
{
    [Required]
    public Guid Id { get; set; }
}
