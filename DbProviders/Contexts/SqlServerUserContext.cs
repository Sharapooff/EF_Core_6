
using Microsoft.EntityFrameworkCore;
using DbProviders.Models;

namespace DbProviders.Contexts
{
    internal class SqlServerUserContext : DbContext
    {
        public DbSet<User> Users => Set<User>(); //{ get; set; } = null!; //{ get; set; }
                                                 //
        //передача в конструктор базового класса объекта DbContextOptions,
        //который инкапсулирует параметры конфигурации
        public SqlServerUserContext(DbContextOptions<SqlServerUserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        //или через OnConfiguring
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=UsersCURD; Trusted_Connection=True;");
        //}
    }
}
