using final_project_backend.Models.request;
using final_project_backend.Models.response;
using final_project_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    /// <summary>
    /// Returns a list of all clients.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ClientListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _clientService.GetAllAsync();
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Returns a single client by its ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _clientService.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Creates a new client. Requires Admin role.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _clientService.CreateAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Updates an existing client. Id must be provided in the request body. Requires Admin role.
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update([FromBody] UpdateClientRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _clientService.UpdateAsync(request);
        return StatusCode(result.StatusCode, result);
    }
}
