using SanayiCebimdeBackend.Application.DTOs;

namespace SanayiCebimdeBackend.Application.Interfaces
{
    public interface IUstalarService
    {
        Task<List<UstaDto>> GetAllUstalarAsync();
    }
}
