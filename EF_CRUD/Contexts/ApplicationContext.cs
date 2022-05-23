using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using EF_CRUD.Models;

namespace EF_CRUD.Contexts
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>(); //{ get; set; }
        public ApplicationContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=UsersCURD; Trusted_Connection=True;");
        }
    }

}

