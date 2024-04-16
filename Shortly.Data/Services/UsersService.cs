using Microsoft.EntityFrameworkCore;
using Shortly.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortly.Data.Services
{
    public class UsersService(AppDbContext appDbContext) : IUsersService
    {
        private readonly AppDbContext _appDbContext = appDbContext;


        public async Task<User> AddAsync(User user)
        {
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetByIdAsync(int id)
        { 
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var users = await _appDbContext.Users.Include(n => n.Urls).ToListAsync();
            return users;
        }

        public async Task<User> UpdateAsync(int id, User user)
        {
            var userDb = await GetByIdAsync(id);

            if (userDb != null)
            {
                userDb.Email = user.Email;
                userDb.FullName = user.FullName;

                await _appDbContext.SaveChangesAsync();
            }
            return userDb;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _appDbContext.Users.Remove(user);
                await _appDbContext.SaveChangesAsync();
            }
        }

    }
}
