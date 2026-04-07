using System.ComponentModel.DataAnnotations;
using final_project_backend.DTOs;

namespace final_project_backend.Models;

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = null!;

    public Guid? AssignedStaffId { get; set; }
    public User? AssignedStaff { get; set; }

    public DateTime ScheduledDate { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

    [MaxLength(2000)]
    public string Notes { get; set; } = string.Empty;

    [Required]
    public decimal PriceAtBooking { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
