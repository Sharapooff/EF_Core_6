using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using MigrationApplication.Models;

namespace MigrationApplication.Contexts
{
    internal class InOnConfiguringContext : DbContext
    {
        public DbSet<User> Users => Set<User>(); //{ get; set; }
        public InOnConfiguringContext()
        {
            //Database.EnsureCreated(); //при миграции должен быть закоментирован
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=UsersMigration; Trusted_Connection=True;");
        }
    }

}

