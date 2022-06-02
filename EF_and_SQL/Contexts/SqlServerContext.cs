
using Microsoft.EntityFrameworkCore;
using EF_and_SQL.Models;

namespace EF_and_SQL.Contexts
{
    internal class SqlServerContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; } //{ get; set; } = null!; //{ get; set; }                                                 
        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        /* Проецирование хранимой функции на метод класса
         * Метод GetUsersByAge(), который соответствует хранимой функции в БД.Он принимает параметр типа int и возвращает объект IQueryable<User>.
         * Этот метод с помощью встроенного в классе DbContext метода FromExpression вызывает GetUsersByAge(age).
         * Далее в переопрепределенном методе OnModelCreating() класса контекста нам надо зарегистрировать метод
         * GetUsersByAge с помощью вызова HasDbFunction():
         */
        public IQueryable<User> GetUsersByAge(int age) => FromExpression(() => GetUsersByAge(age));
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDbFunction(() => GetUsersByAge(default));
        }
        
    }
}
