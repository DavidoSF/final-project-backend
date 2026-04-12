using final_project_backend.Data;
using final_project_backend.DTOs;
using final_project_backend.Models;
using final_project_backend.Models.request;
using final_project_backend.Models.response;
using final_project_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Services.Implementations;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _db;

    public AppointmentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AppointmentListResponse> GetAllAsync(
        AppointmentStatus? status,
        Guid? serviceId,
        Guid? clientId)
    {
        var query = _db.Appointments
            .AsNoTracking()
            .Include(a => a.Client)
            .Include(a => a.Service)
            .Include(a => a.AssignedStaff)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(a => a.Status == status.Value);

        if (serviceId.HasValue)
            query = query.Where(a => a.ServiceId == serviceId.Value);

        if (clientId.HasValue)
            query = query.Where(a => a.ClientId == clientId.Value);

        var appointments = await query
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return new AppointmentListResponse
        {
            Success = true,
            Message = "Appointments retrieved successfully.",
            StatusCode = 200,
            Data = appointments.Select(MapToDto).ToList()
        };
    }

    public async Task<AppointmentResponse> GetByIdAsync(Guid id)
    {
        var appointment = await FindAppointmentAsync(id);
        if (appointment is null)
            return NotFoundResponse("Appointment not found.");

        return new AppointmentResponse
        {
            Success = true,
            Message = "Appointment retrieved successfully.",
            StatusCode = 200,
            Data = MapToDto(appointment)
        };
    }

    public async Task<AppointmentResponse> CreateAsync(CreateAppointmentRequest request)
    {
        var client = await _db.Clients.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.ClientId);
        if (client is null)
            return ErrorResponse("Client not found.", 404);

        var service = await _db.Services.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.ServiceId);
        if (service is null)
            return ErrorResponse("Service not found.", 404);

        User? assignedStaff = null;
        if (request.AssignedStaffId.HasValue)
        {
            assignedStaff = await FindAssignableStaffAsync(request.AssignedStaffId.Value);
            if (assignedStaff is null)
                return ErrorResponse("Assigned staff member not found.", 404);
        }

        var appointment = new Appointment
        {
            ClientId = request.ClientId,
            ServiceId = request.ServiceId,
            AssignedStaffId = request.AssignedStaffId,
            ScheduledDate = request.ScheduledDate,
            Notes = request.Notes.Trim(),
            PriceAtBooking = service.Price,
            Status = AppointmentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _db.Appointments.Add(appointment);
        await _db.SaveChangesAsync();

        appointment.Client = client;
        appointment.Service = service;
        appointment.AssignedStaff = assignedStaff;

        return new AppointmentResponse
        {
            Success = true,
            Message = "Appointment created successfully.",
            StatusCode = 201,
            Data = MapToDto(appointment)
        };
    }

    public async Task<AppointmentResponse> UpdateStatusAsync(Guid id, UpdateAppointmentStatusRequest request)
    {
        var appointment = await _db.Appointments
            .Include(a => a.Client)
            .Include(a => a.Service)
            .Include(a => a.AssignedStaff)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appointment is null)
            return NotFoundResponse("Appointment not found.");

        if (!IsStatusTransitionAllowed(appointment.Status, request.Status))
            return ErrorResponse("Invalid status transition.", 400);

        appointment.Status = request.Status;
        await _db.SaveChangesAsync();

        return new AppointmentResponse
        {
            Success = true,
            Message = "Appointment status updated successfully.",
            StatusCode = 200,
            Data = MapToDto(appointment)
        };
    }

    public async Task<AppointmentResponse> AssignStaffAsync(Guid id, AssignAppointmentStaffRequest request)
    {
        var appointment = await _db.Appointments
            .Include(a => a.Client)
            .Include(a => a.Service)
            .Include(a => a.AssignedStaff)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appointment is null)
            return NotFoundResponse("Appointment not found.");

        User? assignedStaff = null;
        if (request.AssignedStaffId.HasValue)
        {
            assignedStaff = await FindAssignableStaffAsync(request.AssignedStaffId.Value);
            if (assignedStaff is null)
                return ErrorResponse("Assigned staff member not found.", 404);
        }

        appointment.AssignedStaffId = request.AssignedStaffId;
        appointment.AssignedStaff = assignedStaff;
        await _db.SaveChangesAsync();

        return new AppointmentResponse
        {
            Success = true,
            Message = "Appointment staff assignment updated successfully.",
            StatusCode = 200,
            Data = MapToDto(appointment)
        };
    }

    public async Task<StaffListResponse> GetAssignableStaffAsync()
    {
        var users = await _db.Users
            .AsNoTracking()
            .Where(u => u.IsActive && (u.Role == "Admin" || u.Role == "Staff"))
            .OrderBy(u => u.FullName)
            .ToListAsync();

        return new StaffListResponse
        {
            Success = true,
            Message = "Assignable staff retrieved successfully.",
            StatusCode = 200,
            Data = users.Select(MapToUserDto).ToList()
        };
    }

    private async Task<Appointment?> FindAppointmentAsync(Guid id)
    {
        return await _db.Appointments
            .AsNoTracking()
            .Include(a => a.Client)
            .Include(a => a.Service)
            .Include(a => a.AssignedStaff)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    private async Task<User?> FindAssignableStaffAsync(Guid id)
    {
        return await _db.Users.FirstOrDefaultAsync(u =>
            u.Id == id &&
            u.IsActive &&
            (u.Role == "Admin" || u.Role == "Staff"));
    }

    private static bool IsStatusTransitionAllowed(AppointmentStatus currentStatus, AppointmentStatus nextStatus)
    {
        return currentStatus switch
        {
            AppointmentStatus.Pending => nextStatus is AppointmentStatus.Approved or AppointmentStatus.Rejected,
            AppointmentStatus.Approved => nextStatus is AppointmentStatus.Completed or AppointmentStatus.Cancelled,
            _ => false
        };
    }

    private static AppointmentDto MapToDto(Appointment appointment) => new()
    {
        Id = appointment.Id,
        ClientId = appointment.ClientId,
        ClientName = appointment.Client?.FullName ?? string.Empty,
        ServiceId = appointment.ServiceId,
        ServiceName = appointment.Service?.Name ?? string.Empty,
        AssignedStaffId = appointment.AssignedStaffId,
        AssignedStaffName = appointment.AssignedStaff?.FullName ?? "Unassigned",
        ScheduledDate = appointment.ScheduledDate,
        Status = appointment.Status,
        Notes = appointment.Notes,
        PriceAtBooking = appointment.PriceAtBooking,
        CreatedAt = appointment.CreatedAt
    };

    private static UserDto MapToUserDto(User user) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        Role = user.Role,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt
    };

    private static AppointmentResponse NotFoundResponse(string message) => ErrorResponse(message, 404);

    private static AppointmentResponse ErrorResponse(string message, int statusCode) => new()
    {
        Success = false,
        Message = message,
        StatusCode = statusCode
    };
}
