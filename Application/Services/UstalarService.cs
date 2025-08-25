﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<List<UstaDto>> GetAllUstalarAsync()
        {
            try
            {


                var ustalar = await _context.Ustalar
                    .AsNoTracking()
                    .ToListAsync();


                return ustalar.Select(u => new UstaDto
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
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving ustalar: {ex.Message}");
                return new List<UstaDto>(); // Return an empty list in case of error
            }
        }


    }
}
