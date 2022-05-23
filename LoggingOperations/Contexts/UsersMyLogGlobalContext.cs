using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using LoggingOperations.Models;
using LoggingOperations.Classes;
using Microsoft.Extensions.Logging;

namespace LoggingOperations.Contexts
{
    internal class UsersMyLogGlobalContext : DbContext
    { 
        public DbSet<User> Users => Set<User>(); //{ get; set; } = null!; //{ get; set; }
        public UsersMyLogGlobalContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=UsersCURD; Trusted_Connection=True;");
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }
        // устанавливаем фабрику логгера
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(new MyLoggerProvider());    // указываем наш провайдер логгирования
        });
    }
}
