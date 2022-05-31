using RelationshipsModels.Models;
using RelationshipsModels.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

var builder_SqlServer = new ConfigurationBuilder();
builder_SqlServer.SetBasePath(Directory.GetCurrentDirectory());// установка пути к текущему каталогу
builder_SqlServer.AddJsonFile("appsettings.json");// получаем конфигурацию из файла appsettings.json
var config = builder_SqlServer.Build();// создаем конфигурацию
string connectionString_SqlServer = config.GetConnectionString("SqlServer");// получаем строку подключения
var optionsBuilder_SqlServer = new DbContextOptionsBuilder<SqlServerAppContext>();
var options_SqlServer = optionsBuilder_SqlServer.UseSqlServer(connectionString_SqlServer).Options;

/* Через навигационные свойства мы можем загружать связанные данные. И здесь у нас три стратегии загрузки:
        - Eager loading (жадная загрузка) (с помощью метода Include(), в который передается навигационное свойство)
        - Explicit loading (явная загрузка) (с помощью метода Load())
        - Lazy loading (ленивая загрузка через определение virtual навигационного свойства)
*/

// добавление данных
using (SqlServerAppContext db = new SqlServerAppContext(options_SqlServer))
{
    // пересоздадим базу данных
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    Position manager = new Position { Name = "Manager" };
    Position developer = new Position { Name = "Developer" };
    db.Positions.AddRange(manager, developer);

    City washington = new City { Name = "Washington" };
    db.Cities.Add(washington);

    Country usa = new Country { Name = "USA", Capital = washington };
    db.Countries.Add(usa);

    Company microsoft = new Company { Name = "Microsoft", Country = usa };
    Company google = new Company { Name = "Google", Country = usa };
    db.Companies.AddRange(microsoft, google);

    User tom = new User { Name = "Tom", Company = microsoft, Position = manager };
    User bob = new User { Name = "Bob", Company = google, Position = developer };
    User alice = new User { Name = "Alice", Company = microsoft, Position = developer };
    User kate = new User { Name = "Kate", Company = google, Position = manager };
    db.Users.AddRange(tom, bob, alice, kate);

    UserProfile profile1 = new UserProfile { Info = "Tom", User = tom };
    UserProfile profile2 = new UserProfile { Info = "Alice", User = bob };
    db.UserProfiles.AddRange(profile1, profile2);

    Course algorithms = new Course { Name = "Алгоритмы" };
    Course basics = new Course { Name = "Основы программирования" };
    db.Courses.AddRange(algorithms, basics);

    // добавляем к студентам курсы
    tom.Courses.Add(algorithms);
    tom.Courses.Add(basics);
    alice.Courses.Add(algorithms);
    bob.Courses.Add(basics);
    //или так
    //algorithms.Students.AddRange(new List<Student>() { tom, bob });

    db.SaveChanges();
}
// получение данных
//--------------------------- Eager loading (жадная загрузка) (с помощью метода Include(), в который передается навигационное свойство) ---------------------------
Console.WriteLine("---------- Eager loading (жадная загрузка) ------------");
using (SqlServerAppContext db = new SqlServerAppContext(options_SqlServer))
{
    {
        // получаем компании------------------------------------
        //    companies = db.Companies.Include(c => c.Users).ToList(); // добавляем данные по пользователям                    
        //    foreach (var company in companies)
        //    {
        //        Console.WriteLine($" * {company.Name}");
        //        // выводим сотрудников компании
        //        foreach (var user in company.Users)
        //            Console.WriteLine($"       - {user.Name}");
        //    }
    }
    // получаем пользователей
    var users = db.Users
                    .Include(u => u.Company)  // добавляем данные по компаниям
                        .ThenInclude(comp => comp.Country)      // к компании добавляем страну 
                            .ThenInclude(count => count.Capital)    // к стране добавляем столицу
                    .Include(u => u.Position) // добавляем данные по должностям
                    .ToList();
    foreach (var useR in users)
    {
        Console.WriteLine($"{useR.Name} - {useR.Position.Name}");
        Console.WriteLine($"{useR.Company?.Name} - {useR.Company?.Country.Name} - {useR.Company?.Country.Capital.Name}");
        Console.WriteLine("----------------------");
    }
//---------------------------  Explicit loading (явная загрузка) ---------------------------
Console.WriteLine("----------  Explicit loading (явная загрузка) ------------");
    //db.Users.Load(); //если все
    Console.WriteLine("---------- Load()");
    Company? company = db.Companies.FirstOrDefault();
    if (company != null)
    {
        db.Users.Where(u => u.CompanyId == company.Id).Load();
        Console.WriteLine($"Company: {company.Name}");
        foreach (var p in company.Users)
            Console.WriteLine($"User: {p.Name}");
    }
    //Для загрузки связанных данных мы также можем использовать методы Collection() и Reference.
    //Метод Collection применяется, если навигационное свойство представляет коллекцию.
    Console.WriteLine("---------- Collection()");
    company = db.Companies.FirstOrDefault();
    if (company != null)
    {
        db.Entry(company).Collection(t => t.Users).Load();
        Console.WriteLine($"Company: {company.Name}");
        foreach (var p in company.Users)
            Console.WriteLine($"User: {p.Name}");
    }
    //Если навигационное свойство представляет одиночный объект, то можно применять метод Reference:
    Console.WriteLine("---------- Collection() и Reference");
    User? user_ = db.Users.FirstOrDefault();  // получаем первого пользователя
    if (user_ != null)
    {
        db.Entry(user_).Reference(x => x.Company).Load();
        Console.WriteLine($"{user_.Name} - {user_.Company?.Name}");
    }
//------------------------------  Lazy loading (ленивая загрузка) ---------------------------
/* Все навигационные свойства должны быть определены как виртуальные (то есть с модификатором virtual),
 * при этом сами классы моделей должны быть public.
 * Добавить в проект через nuget пакет Microsoft.EntityFrameworkCore.Proxiesy.
 * При конфигурации контекста данных вызвать метод UseLazyLoadingProxies().
 */
Console.WriteLine("----------  Lazy loading (ленивая загрузка) ------------");
    var users_ = db.Users.ToList();
    foreach (User usEr in users_)
        Console.WriteLine($"{usEr.Name} - {usEr.Company?.Name}");
    //---
    Console.WriteLine("");
    var companies = db.Companies.ToList();
    foreach (Company company_ in companies)
    {
        Console.Write($"{company_.Name}:");
        foreach (User uSer in company_.Users)
            Console.Write($"{uSer.Name} ");
        Console.WriteLine();
    }

    //------------------------------ отношения между моделями ---------------------------
    Console.WriteLine("----------  User - UserProfile (1 - 1) ------------");
    /*При удалении надо учитывать следующее: так как объект UserProfile требует наличие объекта User и зависит от этого объекта,
     * то при удалении связанного объекта User также будет удален и связанный с ним объект UserProfile.
     * Если же будет удален объект UserProfile, на объект User это никак не повлияет.
     * создается уникальный индекс. И этот индекс гарантирует, что только одна зависимая сущность (здесь UserProfile) может быть связана
     * с одной главной сущностью (здесь сущность User)
     */
    foreach (User _user in db.Users.Include(u => u.Profile).ToList())
    {
        Console.WriteLine($"UserInfo: {_user.Profile?.Info} Id: {_user.Profile?.Id}");
        Console.WriteLine($"Login: { _user.Name} \n");
    }
    //
    Console.WriteLine("----------  User - Position (1 - n) ------------");
    /*
     * Одна модель хранит ссылку на один объект другой модели, а вторая модель может ссылаться на коллекцию объектов первой модели.
     * Если зависимая сущность (в данном случае User) требует обязательного наличия главной сущности (в данном случае Position), 
     * то на уровне базы данных при удалении главной сущности с помощью каскадного удаления будут удалены и связанные с ней зависимые сущности. 
     */
    foreach (User usER in db.Users.Include(u => u.Position).ToList())
    {
        Console.WriteLine($"UserPosition: {usER.Position?.Name} Id: {usER.Position?.Id}");
        Console.WriteLine($"Login: { usER.Name} \n");
    }
    //
    Console.WriteLine("----------  User - Course (n - n) ------------");
    /*
     * Однако, при создании базы данных в ней будет три таблицы.
     * Удаление же студента или курса из базы данных приведет к тому, что все строки из промежуточной таблицы,
     * которые связаны с удаляемым объектом, также будут удалены
     */
    var courses = db.Courses.Include(c => c.Users).ToList();
    // выводим все курсы
    foreach (var c in courses)
    {
        Console.WriteLine($"Course: {c.Name}");
        // выводим всех юзеров для данного кура
        foreach (User s in c.Users)
            Console.WriteLine($"Name: {s.Name}");
        Console.WriteLine("-------------------");
    }
    //редактирование
    User? alice = db.Users.Include(s => s.Courses).FirstOrDefault(s => s.Name == "Alice");
    Course? algorithms = db.Courses.FirstOrDefault(c => c.Name == "Алгоритмы");
    Course? basics = db.Courses.FirstOrDefault(c => c.Name == "Основы программирования");
    if (alice != null && algorithms != null && basics != null)
    {
        // удаление курса у студента
        alice.Courses.Remove(algorithms);
        // добавление нового курса студенту
        alice.Courses.Add(basics);
        db.SaveChanges();
    }
    //удаление
    User? user = db.Users.FirstOrDefault();
    if (user != null)
    {
        db.Users.Remove(user);
        db.SaveChanges();
    }

    /* Так же существуют 
     * Комплексные типы.------------------------------------------------------------------
     * Атрибут OwnedAttribute позволяет установить зависимый тип для основного.
     * По сути одна модель (тип) включаетс в себя другую модель (тип).
     * Но при создании таблицы, создается одна общая для них таблица.
     * public class User
        {
            public int Id { get; set; }
            public string? Login { get; set; }
            public string? Password { get; set; }
            public UserProfile? Profile { get; set; }
        }
        [Owned]
        public class UserProfile
        {
            public string? Name { get; set; }
            public int Age { get; set; }
        }
    // и контекст:
        public DbSet<User> Users { get; set; } = null!;

    //добавление данных
        User user1 = new User
        {
            Login = "login1",
            Password = "pass1234",
            Profile = new UserProfile { Age = 23, Name = "Tom" }
        };
        User user2 = new User
        {
            Login = "login2",
            Password = "5678word2",
            Profile = new UserProfile { Age = 27, Name = "Alice" }
        };
        db.Users.AddRange(user1, user2);
        db.SaveChanges();
     */

    /*
     * Иерархические данные.----------------------------------------------------------------
     * 
     * public class MenuItem
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public int? ParentId { get; set; }
            public MenuItem? Parent { get; set; }
            public List<MenuItem> Children { get; set; } = new();
        }
    // и контекст:
      public DbSet<MenuItem> MenuItems { get; set; } = null!;
        
    //добавление
    MenuItem file = new MenuItem { Title = "File" };
    MenuItem edit = new MenuItem { Title = "Edit" };
    MenuItem open = new MenuItem { Title = "Open", Parent = file };
    MenuItem save = new MenuItem { Title = "Save", Parent = file };
     */

}


