namespace final_project_backend.DTOs;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public Guid? AssignedStaffId { get; set; }
    public string AssignedStaffName { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public decimal PriceAtBooking { get; set; }
    public DateTime CreatedAt { get; set; }
}
