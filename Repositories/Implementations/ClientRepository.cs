using final_project_backend.Data;
using final_project_backend.Models;
using final_project_backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Repositories.Implementations;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _db;

    public ClientRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Client>> GetAllAsync()
        => await _db.Clients.ToListAsync();

    public async Task<Client?> GetByIdAsync(Guid id)
        => await _db.Clients.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
        => await _db.Clients.AnyAsync(c => c.Email == email && c.Id != excludeId);

    public async Task<Client> CreateAsync(Client client)
    {
        _db.Clients.Add(client);
        await _db.SaveChangesAsync();
        return client;
    }

    public async Task<Client?> UpdateAsync(Guid id, Client updated)
    {
        var existing = await _db.Clients.FirstOrDefaultAsync(c => c.Id == id);
        if (existing is null) return null;

        existing.FullName = updated.FullName;
        existing.Email = updated.Email;
        existing.PhoneNumber = updated.PhoneNumber;
        existing.CompanyName = updated.CompanyName;

        await _db.SaveChangesAsync();
        return existing;
    }
}
