using final_project_backend.Models.request;
using final_project_backend.Models.response;
using final_project_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServiceController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    /// <summary>
    /// Returns a list of all services.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ServiceListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _serviceService.GetAllAsync();
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Returns a single service by its ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _serviceService.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Creates a new service. Requires Admin role.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateServiceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _serviceService.CreateAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Updates an existing service. Requires Admin role.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateServiceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _serviceService.UpdateAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Deletes a service by its ID. Requires Admin role.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CommonResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _serviceService.DeleteAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Toggles the active/inactive status of a service. Requires Admin role.
    /// </summary>
    [HttpPatch("{id:guid}/toggle-active")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleActive(Guid id)
    {
        var result = await _serviceService.ToggleActiveAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
