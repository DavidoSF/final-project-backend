using final_project_backend.Models.request;
using final_project_backend.Models.response;

namespace final_project_backend.Services.Interfaces;

public interface IServiceService
{
    Task<ServiceListResponse> GetAllAsync();
    Task<ServiceResponse> GetByIdAsync(Guid id);
    Task<ServiceResponse> CreateAsync(CreateServiceRequest request);
    Task<ServiceResponse> UpdateAsync(Guid id, UpdateServiceRequest request);
    Task<CommonResponseModel> DeleteAsync(Guid id);
    Task<ServiceResponse> ToggleActiveAsync(Guid id);
}
