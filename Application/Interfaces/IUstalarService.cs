using SanayiCebimdeBackend.Application.DTOs;

namespace SanayiCebimdeBackend.Application.Interfaces
{
    public interface IUstalarService
    {
        //Task<List<UstaDto>> GetAllUstalarAsync();
        Task<(List<UstaDto> Items, int TotalCount)> GetPagedUstalarsAsync(int pageNumber, int pageSize);
    }
}
