using final_project_backend.Data;
using final_project_backend.Models;
using final_project_backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Repositories.Implementations;

public class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _db;

    public ServiceRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Service>> GetAllAsync()
        => await _db.Services.ToListAsync();

    public async Task<Service?> GetByIdAsync(Guid id)
        => await _db.Services.FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Service> CreateAsync(Service service)
    {
        _db.Services.Add(service);
        await _db.SaveChangesAsync();
        return service;
    }

    public async Task<Service?> UpdateAsync(Guid id, Service updated)
    {
        var existing = await _db.Services.FirstOrDefaultAsync(s => s.Id == id);
        if (existing is null) return null;

        existing.Name = updated.Name;
        existing.Description = updated.Description;
        existing.Price = updated.Price;
        existing.DurationInMinutes = updated.DurationInMinutes;

        await _db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var service = await _db.Services.FirstOrDefaultAsync(s => s.Id == id);
        if (service is null) return false;

        _db.Services.Remove(service);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Service?> ToggleActiveAsync(Guid id)
    {
        var service = await _db.Services.FirstOrDefaultAsync(s => s.Id == id);
        if (service is null) return null;

        service.IsActive = !service.IsActive;
        await _db.SaveChangesAsync();
        return service;
    }
}
