using Microsoft.EntityFrameworkCore;
using Shortly.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortly.Data.Services
{
    public class UsersService(AppDbContext appDbContext):IUsersService
    {
        private readonly AppDbContext _appDbContext = appDbContext;


        public User Add(User user)
        {
            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();

            return user;
        }

        public User GetById(int id)
        { 
            var user = _appDbContext.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public List<User> GetUsers() => [.. _appDbContext.Users.Include(n => n.Urls)];

        public User Update(int id, User user)
        {
            var userDb = GetById(id);

            if (userDb != null)
            {
                userDb.Email = user.Email;
                userDb.FullName = user.FullName;

                _appDbContext.SaveChanges();
            }
            return userDb;
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            if (user != null)
            {
                _appDbContext.Users.Remove(user);
                _appDbContext.SaveChanges();
            }
        }

    }
}
