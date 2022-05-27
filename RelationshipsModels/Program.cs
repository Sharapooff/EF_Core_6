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
    foreach (var user in users)
    {
        Console.WriteLine($"{user.Name} - {user.Position.Name}");
        Console.WriteLine($"{user.Company?.Name} - {user.Company?.Country.Name} - {user.Company?.Country.Capital.Name}");
        Console.WriteLine("----------------------");
    }
//---------------------------  Explicit loading (явная загрузка) ---------------------------
Console.WriteLine("----------  Explicit loading (явная загрузка) ------------");
    //db.Users.Load(); //если все
    Console.WriteLine("---------- Load()");
    Company company = db.Companies.FirstOrDefault();
    db.Users.Where(p => p.CompanyId == company.Id).Load();
    Console.WriteLine($"Company: {company.Name}");
    foreach (var p in company.Users)
        Console.WriteLine($"User: {p.Name}");
    //Для загрузки связанных данных мы также можем использовать методы Collection() и Reference.
    //Метод Collection применяется, если навигационное свойство представляет коллекцию.
    Console.WriteLine("---------- Collection()");
    company = db.Companies.FirstOrDefault();
    db.Entry(company).Collection(t => t.Users).Load();
    Console.WriteLine($"Company: {company.Name}");
    foreach (var p in company.Users)
        Console.WriteLine($"User: {p.Name}");
    //Если навигационное свойство представляет одиночный объект, то можно применять метод Reference:
    Console.WriteLine("---------- Collection() и Reference");
    User user_ = db.Users.FirstOrDefault();  // получаем первого пользователя
    db.Entry(user_).Reference(x => x.Company).Load();
    Console.WriteLine($"{user_.Name} - {user_.Company?.Name}");

//------------------------------  Lazy loading (ленивая загрузка) ---------------------------
Console.WriteLine("----------  Lazy loading (ленивая загрузка) ------------");
    var users_ = db.Users.ToList();
    foreach (User user in users_)
        Console.WriteLine($"{user.Name} - {user.Company?.Name}");
    //---
    Console.WriteLine("");
    var companies = db.Companies.ToList();
    foreach (Company company_ in companies)
    {
        Console.Write($"{company_.Name}:");
        foreach (User user in company_.Users)
            Console.Write($"{user.Name} ");
        Console.WriteLine();
    }

    //------------------------------ отношения между моделями ---------------------------
    Console.WriteLine("----------  User - UserProfile (1 - 1) ------------");
    foreach (User user in db.Users.Include(u => u.Profile).ToList())
    {
        Console.WriteLine($"UserInfo: {user.Profile?.Info} Id: {user.Profile?.Id}");
        Console.WriteLine($"Login: { user.Name} \n");
    }
    //
    Console.WriteLine("----------  User - Position (1 - n) ------------");
    foreach (User user in db.Users.Include(u => u.Position).ToList())
    {
        Console.WriteLine($"UserPosition: {user.Position?.Name} Id: {user.Position?.Id}");
        Console.WriteLine($"Login: { user.Name} \n");
    }
    //
    Console.WriteLine("----------  User - Position (n - n) ------------");
    var courses = db.Courses.Include(c => c.Users).ToList();
    // выводим все курсы
    foreach (var c in courses)
    {
        Console.WriteLine($"Course: {c.Name}");
        // выводим всех студентов для данного кура
        foreach (User s in c.Users)
            Console.WriteLine($"Name: {s.Name}");
        Console.WriteLine("-------------------");
    }
}


