
using Microsoft.EntityFrameworkCore;
using Transaction.Models;

namespace Transaction.Contexts
{
    internal class SqlServerAppContext : DbContext
    {
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Company> Companies => Set<Company>(); //{ get; set; } = null!; //{ get; set; }
        public DbSet<User> Users => Set<User>(); //{ get; set; } = null!; //{ get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Course> Courses { get; set; }

        public SqlServerAppContext(DbContextOptions<SqlServerAppContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(); //для ленивой загрузки EF
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Инициализация БД начальными данными
            //modelBuilder.Entity<User>().HasData(
            //    new User { Id = 1, Name = "Tom", Age = 23 },
            //    new User { Id = 2, Name = "Alice", Age = 26 },
            //    new User { Id = 3, Name = "Sam", Age = 28 }
            //);
            //или боле слоными, связанными обьектами
            // определяем компании
            //Company microsoft = new Company { Id = 1, Name = "Microsoft" };
            //Company google = new Company { Id = 2, Name = "Google" };
            ////// определяем пользователей
            //User tom = new User { Id = 1, Name = "Tom", Age = 23, CompanyId = microsoft.Id };
            //User alice = new User { Id = 2, Name = "Alice", Age = 26, CompanyId = microsoft.Id };
            //User sam = new User { Id = 3, Name = "Sam", Age = 28, CompanyId = google.Id };
            //User bob = new User { Name = "Bob", Age = 31, CompanyId = google.Id };
            //User kate = new User { Name = "Kate", Age = 32, CompanyId = google.Id };

            ////// добавляем данные для обеих сущностей
            //modelBuilder.Entity<Company>().HasData(microsoft, google);
            //modelBuilder.Entity<User>().HasData(tom, alice, sam, bob, alice, kate);

            base.OnModelCreating(modelBuilder);
        }


    }
}
