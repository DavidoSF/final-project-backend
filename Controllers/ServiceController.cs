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
    /// Updates an existing service. Id must be provided in the request body. Requires Admin role.
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateServiceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _serviceService.UpdateAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Deletes a service. Id must be provided in the request body. Requires Admin role.
    /// </summary>
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CommonResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromBody] ServiceIdRequest request)
    {
        var result = await _serviceService.DeleteAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Toggles the active/inactive status of a service. Id must be provided in the request body. Requires Admin role.
    /// </summary>
    [HttpPatch("toggle-active")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleActive([FromBody] ServiceIdRequest request)
    {
        var result = await _serviceService.ToggleActiveAsync(request);
        return StatusCode(result.StatusCode, result);
    }
}
