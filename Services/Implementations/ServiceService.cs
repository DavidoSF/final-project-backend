using final_project_backend.DTOs;
using final_project_backend.Models;
using final_project_backend.Models.request;
using final_project_backend.Models.response;
using final_project_backend.Repositories.Interfaces;
using final_project_backend.Services.Interfaces;

namespace final_project_backend.Services.Implementations;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _repo;

    public ServiceService(IServiceRepository repo)
    {
        _repo = repo;
    }

    public async Task<ServiceListResponse> GetAllAsync()
    {
        var services = await _repo.GetAllAsync();
        return new ServiceListResponse
        {
            Success = true,
            Message = "Services retrieved successfully.",
            StatusCode = 200,
            Data = services.Select(MapToDto).ToList()
        };
    }

    public async Task<ServiceResponse> GetByIdAsync(Guid id)
    {
        var service = await _repo.GetByIdAsync(id);
        if (service is null)
            return new ServiceResponse
            {
                Success = false,
                Message = "Service not found.",
                StatusCode = 404
            };

        return new ServiceResponse
        {
            Success = true,
            Message = "Service retrieved successfully.",
            StatusCode = 200,
            Data = MapToDto(service)
        };
    }

    public async Task<ServiceResponse> CreateAsync(CreateServiceRequest request)
    {
        var service = new Service
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            DurationInMinutes = request.DurationInMinutes,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repo.CreateAsync(service);
        return new ServiceResponse
        {
            Success = true,
            Message = "Service created successfully.",
            StatusCode = 201,
            Data = MapToDto(created)
        };
    }

    public async Task<ServiceResponse> UpdateAsync(Guid id, UpdateServiceRequest request)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing is null)
            return new ServiceResponse
            {
                Success = false,
                Message = "Service not found.",
                StatusCode = 404
            };

        var patch = new Service
        {
            Name = request.Name ?? existing.Name,
            Description = request.Description ?? existing.Description,
            Price = request.Price ?? existing.Price,
            DurationInMinutes = request.DurationInMinutes ?? existing.DurationInMinutes
        };

        var updated = await _repo.UpdateAsync(id, patch);
        return new ServiceResponse
        {
            Success = true,
            Message = "Service updated successfully.",
            StatusCode = 200,
            Data = MapToDto(updated!)
        };
    }

    public async Task<CommonResponseModel> DeleteAsync(Guid id)
    {
        var deleted = await _repo.DeleteAsync(id);
        if (!deleted)
            return new CommonResponseModel
            {
                Success = false,
                Message = "Service not found.",
                StatusCode = 404
            };

        return new CommonResponseModel
        {
            Success = true,
            Message = "Service deleted successfully.",
            StatusCode = 200
        };
    }

    public async Task<ServiceResponse> ToggleActiveAsync(Guid id)
    {
        var toggled = await _repo.ToggleActiveAsync(id);
        if (toggled is null)
            return new ServiceResponse
            {
                Success = false,
                Message = "Service not found.",
                StatusCode = 404
            };

        return new ServiceResponse
        {
            Success = true,
            Message = $"Service {(toggled.IsActive ? "activated" : "deactivated")} successfully.",
            StatusCode = 200,
            Data = MapToDto(toggled)
        };
    }

    private static ServiceDto MapToDto(Service service) => new()
    {
        Id = service.Id,
        Name = service.Name,
        Description = service.Description,
        Price = service.Price,
        DurationInMinutes = service.DurationInMinutes,
        IsActive = service.IsActive,
        CreatedAt = service.CreatedAt
    };
}
