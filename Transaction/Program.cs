
using Transaction.Contexts;
using Transaction.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

var builder_SqlServer = new ConfigurationBuilder();
builder_SqlServer.SetBasePath(Directory.GetCurrentDirectory());// установка пути к текущему каталогу
builder_SqlServer.AddJsonFile("appsettings.json");// получаем конфигурацию из файла appsettings.json
var config = builder_SqlServer.Build();// создаем конфигурацию
string connectionString_SqlServer = config.GetConnectionString("SqlServer");// получаем строку подключения
var optionsBuilder_SqlServer = new DbContextOptionsBuilder<SqlServerAppContext>();
var options_SqlServer = optionsBuilder_SqlServer.UseSqlServer(connectionString_SqlServer).Options;

Console.WriteLine("Transaction_________________________");
/*
 * В данном случае и метод db.Database.ExecuteSqlCommand и db.SaveChanges() определяют две разные транзакции. 
 * Однако поскольку мы оборачиваем оба этих метода в блок using (var transaction = db.Database.BeginTransaction())
 * оба этих метода будут объединены в одну транзакцию. И если вызов db.Database.ExecuteSqlCommand пройдет неудачно,
 * то вызов db.SaveChanges() не будет выполняться.
 */
using (SqlServerAppContext db = new SqlServerAppContext(options_SqlServer))
{
    using (var transaction = db.Database.BeginTransaction())
    {
        try
        {            
            db.Users.Add(new User { Age = 34, Name = "Bob", CompanyId = 1 });
            db.SaveChanges();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
        }
    }

    foreach (User p in db.Users.ToList())
        Console.WriteLine("Name: {0}  Age: {1}", p.Name, p.Age);
}