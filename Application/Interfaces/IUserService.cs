using SanayiCebimdeBackend.Application.DTOs;

namespace SanayiCebimdeBackend.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> AuthenticateAsync(string username, string password);
        Task<UserDto> RegisterAsync(UserDto userDto, string password);
        Task<UserDto?> GetByIdAsync(int id);
        // Add CRUD methods as needed
    }
}