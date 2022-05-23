using MigrationApplication.Contexts;
using MigrationApplication.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

Console.WriteLine("InOnConfiguringContext______________:");
using (InOnConfiguringContext db = new InOnConfiguringContext())
{
    var users = db.Users.ToList();
    foreach (User u in users)
    {
        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
    }
}
/* Выше была рассмотрена миграция для контекста данных, который имеет конструктор без параметров
 * и устанавливает настройки подключения в методе OnConfiguring(). Однако мы можем также передавать
 * параметры подключения в контекст данных извне через конструктор с параметром типа DbContextOptions.
 * подробное описание в модели.
 */
Console.WriteLine("InDbContextOptionsContext______________:");

ConfigurationBuilder builder = new ConfigurationBuilder();
// установка пути к текущему каталогу
builder.SetBasePath(Directory.GetCurrentDirectory());
// получаем конфигурацию из файла appsettings.json
builder.AddJsonFile("appsettings.json");
// создаем конфигурацию
var config = builder.Build();
// получаем строку подключения
string connectionString = config.GetConnectionString("DefaultConnection");

var optionsBuilder = new DbContextOptionsBuilder<InDbContextOptionsContext>();
var options = optionsBuilder.UseSqlServer(connectionString).Options;

using (InDbContextOptionsContext db = new InDbContextOptionsContext(options))
{
    var users = db.Users.ToList();
    foreach (User u in users)
    {
        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
    }
}