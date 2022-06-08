
using Microsoft.EntityFrameworkCore;
using LINQtoEntities.Models;

namespace LINQtoEntities.Contexts
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>(); //{ get; set; } = null!; //{ get; set; }
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;

        //передача в конструктор базового класса объекта DbContextOptions, который инкапсулирует параметры конфигурации
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(); //для ленивой загрузки EF
        }
    }
}
