using final_project_backend.Models.request;
using final_project_backend.Models.response;

namespace final_project_backend.Services.Interfaces;

public interface IClientService
{
    Task<ClientListResponse> GetAllAsync();
    Task<ClientResponse> GetByIdAsync(Guid id);
    Task<ClientResponse> CreateAsync(CreateClientRequest request);
    Task<ClientResponse> UpdateAsync(UpdateClientRequest request);
}
