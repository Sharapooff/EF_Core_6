using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ConnectionConfiguration.Models;

namespace ConnectionConfiguration.Contexts
{
    internal class InOnConfiguringContext : DbContext
    {
        public DbSet<User> Users => Set<User>();// { get; set; }
        public InOnConfiguringContext()
        {
            Database.EnsureCreated();
        }
        //объект DbContextOptionsBuilder позволяет создать параметры подключения
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=UsersCURD; Trusted_Connection=True;");
        }
    }
}
