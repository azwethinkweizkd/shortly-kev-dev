using Microsoft.EntityFrameworkCore;
using Shortly.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortly.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Url> Urls { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
