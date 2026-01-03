using Microsoft.EntityFrameworkCore;
using SanayiCebimdeBackend.Application.DTOs;
using SanayiCebimdeBackend.Application.Interfaces;
using SanayiCebimdeBackend.Domain.Entities;
using SanayiCebimdeBackend.Infrastructure.Data;

namespace SanayiCebimdeBackend.Application.Services
{
    public class UstalarService : IUstalarService
    {

        private readonly AppDbContext _context;

        public UstalarService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<UstaDto> Items, int TotalCount)> GetPagedUstalarsAsync(int pageNumber, int pageSize)
        {
            try
            {
                // 1. Sorguyu hazırla
                var query = _context.Ustalar.AsNoTracking();

                // 2. Toplam sayıyı al (Veritabanında çalışır)
                var totalCount = await query.CountAsync();

                // 3. Sadece ilgili sayfadaki "Entity" listesini çek 
                // Bu satır eski çalışan kodunla aynı mantıkta çalışır, sadece parça çeker.
                var pagedEntities = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // 4. Eşlemeyi (Mapping) RAM üzerinde yap (Hata almanı engeller)
                var mappedItems = pagedEntities.Select(u => new UstaDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Profession = u.Profession,
                    Location = u.Location,
                    Rating = u.Rating,
                    Reviews = u.Reviews,
                    Image = u.Image,
                    Description = u.Description,
                    MemberSince = u.MemberSince,
                    TotalJobs = u.TotalJobs,
                    SatisfactionRate = u.SatisfactionRate,
                    Phone = u.Phone,
                    Date = u.Date,
                    ImageURL = u.ImageURL,
                    Yorumlar = u.Yorumlar,
                    Galeri = u.Galeri,
                    Skills = u.Skills
                }).ToList();

                return (mappedItems, totalCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                return (new List<UstaDto>(), 0);
            }
        }


    }
}
