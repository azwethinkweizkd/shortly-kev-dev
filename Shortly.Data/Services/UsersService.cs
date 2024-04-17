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


        public async Task<List<AppUser>> GetUsersAsync()
        {
            var users = await _appDbContext.Users.Include(n => n.Urls).ToListAsync();
            return users;
        }


    }
}
