namespace final_project_backend.DTOs;

public class DashboardOverviewDto
{
    public int TotalClients { get; set; }
    public int TotalServices { get; set; }
    public int PendingAppointments { get; set; }
    public int ApprovedAppointments { get; set; }
    public int CompletedAppointments { get; set; }
    public int CancelledAppointments { get; set; }
    public decimal TotalRevenue { get; set; }
}
