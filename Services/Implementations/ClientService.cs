using final_project_backend.DTOs;
using final_project_backend.Models;
using final_project_backend.Models.request;
using final_project_backend.Models.response;
using final_project_backend.Repositories.Interfaces;
using final_project_backend.Services.Interfaces;

namespace final_project_backend.Services.Implementations;

public class ClientService : IClientService
{
    private readonly IClientRepository _repo;

    public ClientService(IClientRepository repo)
    {
        _repo = repo;
    }

    public async Task<ClientListResponse> GetAllAsync()
    {
        var clients = await _repo.GetAllAsync();
        return new ClientListResponse
        {
            Success = true,
            Message = "Clients retrieved successfully.",
            StatusCode = 200,
            Data = clients.Select(MapToDto).ToList()
        };
    }

    public async Task<ClientResponse> GetByIdAsync(Guid id)
    {
        var client = await _repo.GetByIdAsync(id);
        if (client is null)
            return new ClientResponse
            {
                Success = false,
                Message = "Client not found.",
                StatusCode = 404
            };

        return new ClientResponse
        {
            Success = true,
            Message = "Client retrieved successfully.",
            StatusCode = 200,
            Data = MapToDto(client)
        };
    }

    public async Task<ClientResponse> CreateAsync(CreateClientRequest request)
    {
        if (await _repo.EmailExistsAsync(request.Email))
            return new ClientResponse
            {
                Success = false,
                Message = "A client with this email already exists.",
                StatusCode = 409
            };

        var client = new Client
        {
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            CompanyName = request.CompanyName,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repo.CreateAsync(client);
        return new ClientResponse
        {
            Success = true,
            Message = "Client created successfully.",
            StatusCode = 201,
            Data = MapToDto(created)
        };
    }

    public async Task<ClientResponse> UpdateAsync(UpdateClientRequest request)
    {
        var existing = await _repo.GetByIdAsync(request.Id);
        if (existing is null)
            return new ClientResponse
            {
                Success = false,
                Message = "Client not found.",
                StatusCode = 404
            };

        if (request.Email is not null && request.Email != existing.Email &&
            await _repo.EmailExistsAsync(request.Email, excludeId: request.Id))
            return new ClientResponse
            {
                Success = false,
                Message = "A client with this email already exists.",
                StatusCode = 409
            };

        var patch = new Client
        {
            FullName = request.FullName ?? existing.FullName,
            Email = request.Email ?? existing.Email,
            PhoneNumber = request.PhoneNumber ?? existing.PhoneNumber,
            CompanyName = request.CompanyName ?? existing.CompanyName
        };

        var updated = await _repo.UpdateAsync(request.Id, patch);
        return new ClientResponse
        {
            Success = true,
            Message = "Client updated successfully.",
            StatusCode = 200,
            Data = MapToDto(updated!)
        };
    }

    private static ClientDto MapToDto(Client client) => new()
    {
        Id = client.Id,
        FullName = client.FullName,
        Email = client.Email,
        PhoneNumber = client.PhoneNumber,
        CompanyName = client.CompanyName,
        CreatedAt = client.CreatedAt
    };
}
