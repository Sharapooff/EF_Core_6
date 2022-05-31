
using Microsoft.EntityFrameworkCore;
using Parallelism.Models;

namespace Parallelism.Contexts
{
    internal class MySqlUserContext : DbContext
    {
        public DbSet<User> Users => Set<User>(); //{ get; set; } = null!; //{ get; set; }
                                                 
        //передача в конструктор базового класса объекта DbContextOptions,
        //который инкапсулирует параметры конфигурации
        public MySqlUserContext(DbContextOptions<MySqlUserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        //или через OnConfiguring
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySql("server=localhost;user=root;password=123456789;database=usersdb;",
        //        new MySqlServerVersion(new Version(8, 0, 25)));
        //}
    }
}
