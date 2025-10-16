using Application.Models;
using Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) 
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            _context.users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailAlreadyExistsAsync(string email)
            => await _context.users.AnyAsync(u => u.Email == email);

        public async Task<User?> GetUserByEmailAsync(string email)
            => await _context.users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
