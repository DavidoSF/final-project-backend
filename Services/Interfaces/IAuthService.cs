using final_project_backend.DTOs;

namespace final_project_backend.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}
