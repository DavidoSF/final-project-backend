using System.ComponentModel.DataAnnotations;

namespace final_project_backend.Models;

public class ActivityLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string ActionType { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public Guid? PerformedByUserId { get; set; }
    public User? PerformedByUser { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
