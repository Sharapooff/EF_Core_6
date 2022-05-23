using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using LoggingOperations.Models;

namespace LoggingOperations.Contexts
{
    internal class UsersLogConsoleContext : DbContext 
    {
        public DbSet<User> Users => Set<User>(); //{ get; set; } = null!; //{ get; set; }
        public UsersLogConsoleContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=UsersCURD; Trusted_Connection=True;");
        }
    }
}
