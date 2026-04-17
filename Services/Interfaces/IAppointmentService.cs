using final_project_backend.DTOs;
using final_project_backend.Models.request;
using final_project_backend.Models.response;

namespace final_project_backend.Services.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentListResponse> GetAllAsync(AppointmentStatus? status, Guid? serviceId, Guid? clientId);
    Task<AppointmentResponse> GetByIdAsync(Guid id);
    Task<AppointmentResponse> CreateAsync(CreateAppointmentRequest request);
    Task<AppointmentResponse> UpdateStatusAsync(Guid id, UpdateAppointmentStatusRequest request);
    Task<AppointmentResponse> AssignStaffAsync(Guid id, AssignAppointmentStaffRequest request);
    Task<StaffListResponse> GetAssignableStaffAsync();
}
