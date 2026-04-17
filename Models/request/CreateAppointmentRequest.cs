using System.ComponentModel.DataAnnotations;

namespace final_project_backend.Models.request;

public class CreateAppointmentRequest
{
    [Required]
    public Guid ClientId { get; set; }

    [Required]
    public Guid ServiceId { get; set; }

    public Guid? AssignedStaffId { get; set; }

    [Required]
    public DateTime ScheduledDate { get; set; }

    [MaxLength(2000)]
    public string Notes { get; set; } = string.Empty;
}
