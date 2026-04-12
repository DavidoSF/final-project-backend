using final_project_backend.Models;

namespace final_project_backend.Repositories.Interfaces;

public interface IClientRepository
{
    Task<List<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(Guid id);
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);
    Task<Client> CreateAsync(Client client);
    Task<Client?> UpdateAsync(Guid id, Client updated);
}
