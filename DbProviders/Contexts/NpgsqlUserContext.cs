
using Microsoft.EntityFrameworkCore;
using DbProviders.Models;

namespace DbProviders.Contexts
{
    internal class NpgsqlUserContext : DbContext
    {
        public DbSet<User> Users => Set<User>(); //{ get; set; } = null!; //{ get; set; }
        
        //передача в конструктор базового класса объекта DbContextOptions,
        //который инкапсулирует параметры конфигурации
        public NpgsqlUserContext(DbContextOptions<NpgsqlUserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        //или через OnConfiguring
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=пароль_от_postgres");
        //}
    }
}
