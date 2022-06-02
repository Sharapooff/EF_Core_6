

using EF_and_SQL.Contexts;
using EF_and_SQL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

Console.WriteLine("EF & SQL!___________________________");
var builder_SqlServer = new ConfigurationBuilder();
builder_SqlServer.SetBasePath(Directory.GetCurrentDirectory());// установка пути к текущему каталогу
builder_SqlServer.AddJsonFile("appsettings.json");// получаем конфигурацию из файла appsettings.json
var config = builder_SqlServer.Build();// создаем конфигурацию
string connectionString_SqlServer = config.GetConnectionString("SqlServer");// получаем строку подключения
var optionsBuilder_SqlServer = new DbContextOptionsBuilder<SqlServerContext>();
var options_SqlServer = optionsBuilder_SqlServer.UseSqlServer(connectionString_SqlServer).Options;

using (SqlServerContext db = new SqlServerContext(options_SqlServer))
{
    //заполнение
    using (var transaction = db.Database.BeginTransaction())
    {
        try
        {           
            
            Company microsoft = new Company { Name = "Microsoft" };
            Company google = new Company { Name = "Google" };
            db.Companies.AddRange(microsoft, google);

            User tom = new User { Name = "Tom", Age = 22, Company = microsoft };
            User bob = new User { Name = "Bob", Age = 28, Company = google };
            User alice = new User { Name = "Alice", Age = 31, Company = microsoft };
            User kate = new User { Name = "Kate", Age = 19, Company = google };
            db.Users.AddRange(tom, bob, alice, kate);

            db.SaveChanges();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            Console.WriteLine("transaction.Rollbac!");
            transaction.Rollback();
        }


    }

    //вывод
    foreach (User p in db.Users.ToList())
        Console.WriteLine($"Name: {p.Name}  Age: {p.Age} - Company: {p.Company.Name}");
    //Запросы на SQL
    #region
    /*
     * FromSqlRaw
       Для получения данных из БД у объектов DbSet определен метод FromSqlRaw(), который принимает в качестве параметра sql-выражение и
       набор параметров. В качестве результата FromSqlRaw возвращает набор полученных из бд объектов.
       При этом надо учитывать, что передаваемое в метод FromSqlRaw SQL-выражение не должно извлекать связанные данные,
       а полученные значения должны соответствовать определению какого-либо класса.
     */
    Console.WriteLine("FromSqlRaw__________________________");
    var comps = db.Companies.FromSqlRaw("SELECT * FROM Companies").ToList();
    foreach (var company in comps)
        Console.WriteLine(company.Name);
    // При этом мы можем добавлять к методу другие методы LINQ, которые все вместе будут конкатенироваться в одно общее SQL-выражение:
    comps = db.Companies.FromSqlRaw("SELECT * FROM Companies").OrderBy(x => x.Name).ToList();
    foreach (var company in comps)
        Console.WriteLine(company.Name);
    // Также можно использовать метод Include для подгрузки связанных данных:
    var users = db.Users.FromSqlRaw("SELECT * FROM Users").Include(c => c.Company).ToList();  //laizy:   var users = db.Users.FromSqlRaw("SELECT * FROM Users").ToList();
    foreach (var user in users)
        Console.WriteLine($"{user.Name} - {user.Company.Name}");
    // Параметры
    // Класс SqlParameter из пространства имен System.Data.SqlClient позволяет задать параметр, который затем передается в запрос sql
    SqlParameter param = new SqlParameter("@name", "%Tom%");
    users = db.Users.FromSqlRaw("SELECT * FROM Users WHERE Name LIKE @name", param).ToList();
    foreach (var user in users)
        Console.WriteLine(user.Name);
    // Можем определять параметры как простые переменные:
    var name = "%Tom%";
    users = db.Users.FromSqlRaw("SELECT * FROM Users WHERE Name LIKE {0}", name).ToList();
    var age = 30;
    users = db.Users.FromSqlRaw("SELECT * FROM Users WHERE Age > {0}", age).ToList();
    /*
     * ExecuteSqlRaw    
       Позволяет удалять, обновлять уже существующие или вставлять новые записи.
       ExecuteSqlRaw(), который возвращает количество затронутых командой строк.
     */

    // вставка
    string newComp = "Apple";
    int numberOfRowInserted = db.Database.ExecuteSqlRaw("INSERT INTO Companies (Name) VALUES ({0})", newComp);
    // обновление
    string appleInc = "Apple Inc.";
    string apple = "Apple";
    int numberOfRowUpdated = db.Database.ExecuteSqlRaw("UPDATE Companies SET Name={0} WHERE Name={1}", appleInc, apple);
    // удаление
    int numberOfRowDeleted = db.Database.ExecuteSqlRaw("DELETE FROM Companies WHERE Name={0}", appleInc);
    // Для методов FromSqlRaw и ExecuteSqlRaw в EF Core определены их двойники - методы FromSqlInterpolated() и
    // ExecuteSqlInterpolated() соответственно, которые позволяют использовать интерполяцию строк для передачи параметров.
    users = db.Users.FromSqlInterpolated($"SELECT * FROM Users WHERE Name LIKE {name} AND Age > {age}").ToList();
    //Методы Database.ExecuteSqlInterpolated() и Database.ExecuteSqlRaw имеют асинхронные версии:
    //ExecuteSqlRawAsync
    //ExecuteSqlInterpolatedAsync
    #endregion

    //Хранимые функции
    #region
    Console.WriteLine("Хранимые функции _________________");
    Microsoft.Data.SqlClient.SqlParameter PRparam = new Microsoft.Data.SqlClient.SqlParameter("@age", 30);
    users = db.Users.FromSqlRaw("SELECT * FROM GetUsersByAge (@age)", PRparam).ToList();
    foreach (var u in users)
        Console.WriteLine($"{u.Name} - {u.Age}");
    /* Проецирование хранимой функции на метод класса
     Второй подход предполагает определение в классе контекста метода, который проецируется на хранимую функцию
     и через который можно вызывать данную функцию.
     */
    Console.WriteLine("Проецирование хранимой функции на метод класса _________________");
    var users_ = db.GetUsersByAge(30);   // обращение к хранимой функции
    foreach (var u in users_)
        Console.WriteLine($"{u.Name} - {u.Age}");

    #endregion

    //Хранимые процедуры
    #region
    Console.WriteLine("Хранимая процедура _________________");
    SqlParameter paramSP = new("@name", "Microsoft");
    var usersSP = db.Users.FromSqlRaw("GetUsersByCompany @name", param).ToList();
    foreach (var p in usersSP)
        Console.WriteLine($"{p.Name} - {p.Age}");
    /* Выходные параметры процедуры.
     * В некоторых случаях процедура может возвращать не набор данных из таблиц, а какие - то отдельные данные.
     * Как правило, для этого используются выходные параметры. Например, нам надо получить имя пользователя с максимальным возрастом.
     */
    SqlParameter paramOut = new()
    {
        ParameterName = "@userName",
        SqlDbType = System.Data.SqlDbType.VarChar,
        Direction = System.Data.ParameterDirection.Output,
        Size = 50
    };
    db.Database.ExecuteSqlRaw("GetUserWithMaxAge @userName OUT", paramOut);
    Console.WriteLine(param.Value);
    /*
     * Параметр userName определен как выходной. Так как нам в данном случае не надо возвращать набор данных, который соответствует
     * одной из моделей, то для выполнения запроса используется метод db.Database.ExecuteSqlRaw(). 
     * И после его выполнения через свойство param.Value мы сможем получить значение, переданное через параметр.
     */

    #endregion
}