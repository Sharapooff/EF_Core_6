using Microsoft.EntityFrameworkCore;
using Parallelism.Contexts;
using Parallelism.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Parallelism______________________________");
/* 
 * В обычном режиме Entity Framework Core при обновлении смотрит на Id и если Id записи в таблице совпадает с 
 * Id в передаваемой модели User, то строка в таблице обновляется. 
 * 
 * 
 //* Атрибут ConcurrencyCheck
    Атрибут ConcurrencyCheck позволяет решить проблему параллелизма, когда с одной и той же записью в таблице
    могут работать одновременно несколько пользователей.
    public class User
    {
        public int Id { get; set; }
        [ConcurrencyCheck]
        public string Name { get; set; }
    }

 //* Метод IsConcurrencyToken
    В Fluent API токен параллелизма настраивается с помощью метода IsConcurrencyToken():    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(b => b.Name).IsConcurrencyToken();
    }

 //*  

 //* Аатрибут [Timestamp]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
     
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
  //* Вместо атрибута можно использовать
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(b => b.Timestamp).IsRowVersion();
    }

 */

var builder_SqlServer = new ConfigurationBuilder();
builder_SqlServer.SetBasePath(Directory.GetCurrentDirectory());// установка пути к текущему каталогу
builder_SqlServer.AddJsonFile("appsettings.json");// получаем конфигурацию из файла appsettings.json
var config = builder_SqlServer.Build();// создаем конфигурацию
string connectionString_SqlServer = config.GetConnectionString("SqlServer");// получаем строку подключения
var optionsBuilder_SqlServer = new DbContextOptionsBuilder<SqlServerUserContext>();
var options_SqlServer = optionsBuilder_SqlServer.UseSqlServer(connectionString_SqlServer).Options;

using (SqlServerUserContext db = new SqlServerUserContext(options_SqlServer))
{
    try
    {
        User user = db.Users.FirstOrDefault();
        if (user != null)
        {
            user.Name = "Bob";
            db.SaveChanges();
        }
    }
    catch (DbUpdateConcurrencyException ex)
    {

    }
}