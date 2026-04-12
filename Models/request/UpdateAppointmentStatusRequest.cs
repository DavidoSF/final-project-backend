using System.ComponentModel.DataAnnotations;
using final_project_backend.DTOs;

namespace final_project_backend.Models.request;

public class UpdateAppointmentStatusRequest
{
    [Required]
    public AppointmentStatus Status { get; set; }
}
