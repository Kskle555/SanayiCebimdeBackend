using SanayiCebimdeBackend.Application.Interfaces;
using SanayiCebimdeBackend.Application.DTOs;
using SanayiCebimdeBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SanayiCebimdeBackend.Application.Services
{
    public class UserService : IUserService
    {

        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);

            if (user == null)
                return null;
            else
            {
                return new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                };
            }
        }



        public Task<UserDto> RegisterAsync(UserDto userDto, string password)
        {
            try
            {

            // kayit olma islemi

                var kontrol = _context.Users.Any(u => u.Username == userDto.Username || u.Email == userDto.Email);

                if (kontrol)
                {
                    return Task.FromException<UserDto>(new Exception("Username or Email already exists."));
                }


                    var user = new Domain.Entities.User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = password 
            };

                //git test
            _context.Users.Add(user);
            _context.SaveChanges();
            return Task.FromResult(new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            });
            }
            catch(Exception Ex)
            {
                return Task.FromException<UserDto>(Ex);

            }

        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if(user == null)
                    return null;

                return new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email

                };

            }
            catch(Exception Ex)
            {
                throw;
            }
            
        }
    }
}