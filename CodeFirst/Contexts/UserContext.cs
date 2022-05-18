using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using CodeFirst.Models;

namespace CodeFirst.Contexts
{
    internal class UserContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public UserContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=Users; Trusted_Connection=True;");
        }
    }
}
