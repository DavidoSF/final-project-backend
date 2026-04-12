using final_project_backend.DTOs;
using final_project_backend.Models.request;
using final_project_backend.Models.response;
using final_project_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Controllers;

[ApiController]
[Route("api/appointments")]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(AppointmentListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] AppointmentStatus? status,
        [FromQuery] Guid? serviceId,
        [FromQuery] Guid? clientId)
    {
        var result = await _appointmentService.GetAllAsync(status, serviceId, clientId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _appointmentService.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("staff-members")]
    [ProducesResponseType(typeof(StaffListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAssignableStaff()
    {
        var result = await _appointmentService.GetAssignableStaffAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Staff")]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _appointmentService.CreateAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}/status")]
    [Authorize(Roles = "Admin,Staff")]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateAppointmentStatusRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _appointmentService.UpdateStatusAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}/assign-staff")]
    [Authorize(Roles = "Admin,Staff")]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignStaff(Guid id, [FromBody] AssignAppointmentStaffRequest request)
    {
        var result = await _appointmentService.AssignStaffAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }
}
