using final_project_backend.Models;

namespace final_project_backend.Repositories.Interfaces;

public interface IServiceRepository
{
    Task<List<Service>> GetAllAsync();
    Task<Service?> GetByIdAsync(Guid id);
    Task<Service> CreateAsync(Service service);
    Task<Service?> UpdateAsync(Guid id, Service updated);
    Task<bool> DeleteAsync(Guid id);
    Task<Service?> ToggleActiveAsync(Guid id);
}
