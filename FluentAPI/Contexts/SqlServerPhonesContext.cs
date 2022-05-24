
using Microsoft.EntityFrameworkCore;
using FluentAPI_Annotations.Models;

namespace FluentAPI_Annotations.Contexts
{
    internal class SqlServerPhonesContext : DbContext
    {
        public DbSet<Phone> Phones => Set<Phone>(); //{ get; set; } = null!; //{ get; set; }                                                 
        public SqlServerPhonesContext(DbContextOptions<SqlServerPhonesContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // использование Fluent API
            //объекты Phone будут храниться в таблице Mobiles.
            //Но мы также с ними сможем работать через свойство db.Phones.
            //modelBuilder.Entity<Phone>().ToTable("Mobiles");

            //Если по какой-то сущности нам не надо создавать таблицу, то мы можем ее проигнорировать
            //с помощью метода Ignore()
            //modelBuilder.Ignore<Company>();

            //По умолчанию в Entity Framework первичный ключ должен представлять свойство модели с именем Id
            //или [Имя_класса]Id, например, PhoneId. Чтобы переопределить первичный ключ через Fluent API,
            //надо использовать метод HasKey()
            //modelBuilder.Entity<Phone>().HasKey(p => p.Ident);

            //Чтобы настроить составной первичный ключ, мы можем указать два свойства:
            //modelBuilder.Entity<Phone>().HasKey(p => new { p.Ident, p.Name });

            //Чтобы сопоставить свойство с определенным столбцом, используется метод HasColumnName():
            //modelBuilder.Entity<Phone>().Property(p => p.Name).HasColumnName("PhoneName");

            //Для строк мы модем указать максимальную длину с помощью метода HasMaxLength()
            //modelBuilder.Entity<Phone>().Property(p => p.Name).HasMaxLength(50);

            //Также для строк можно определить, будут ли они храниться в кодировке Unicode:
            //modelBuilder.Entity<Phone>().Property(p => p.Name).IsUnicode(false);

            //Если у нас есть свойство с типом decimal, то мы можем указать для него точность
            //число цифр в числе и число цифр после запятой:
            //допустим, свойство Price - decimal
            //modelBuilder.Entity<Phone>().Property(p => p.Price).HasPrecision(15, 2);

            //По умолчанию EF сам выбирает тип данных в бд, исходя из типа данных свойства. Но мы также можем явно
            //указать, какой тип данных в БД должен использоваться для столбца с помощью метода HasColumnType():
            //modelBuilder.Entity<Phone>().Property(p => p.Name).HasColumnType("varchar");
            //modelBuilder.Entity<User>().Property(u=>u.Name).HasColumnType("varchar(200)");

            //С помощью Fluent API мы можем поместить ряд свойств модели в одну таблицу, а другие свойства связать
            //со столбцами из другой таблицы:
            //modelBuilder.Entity<Phone>().Map(m =>
            //{
            //    m.Properties(p => new { p.Ident, p.Name });
            //    m.ToTable("Mobiles");
            //})
            //.Map(m =>
            // {
            //     m.Properties(p => new { p.Ident, p.Price, p.Discount });
            //     m.ToTable("MobilesInfo");
            // });
            //данные для свойства Name будут храниться в таблице Mobiles, а данные для свойств Price и Discount
            //- в таблице MobilesInfo. И столбец идентификатора будет общим

            //Иногда возникают ситуации, когда надо, наоборот, исключить сущность из модели.
            //Можем использовать Fluent API (modelBuilder.Ignore<Company>()) или аннотации данных ([NotMapped]).

            //Ограничение для столбца
            //modelBuilder.Entity<User>().HasCheckConstraint("Age", "Age > 0 AND Age < 120");

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
            //// определяем пользователей
            //User tom = new User { Id = 1, Name = "Tom", Age = 23, CompanyId = microsoft.Id };
            //User alice = new User { Id = 2, Name = "Alice", Age = 26, CompanyId = microsoft.Id };
            //User sam = new User { Id = 3, Name = "Sam", Age = 28, CompanyId = google.Id };

            //// добавляем данные для обеих сущностей
            //modelBuilder.Entity<Company>().HasData(microsoft, google);
            //modelBuilder.Entity<User>().HasData(tom, alice, sam);

            base.OnModelCreating(modelBuilder);
        }


    }
}
