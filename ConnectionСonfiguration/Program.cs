using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using ConnectionConfiguration.Models;
using ConnectionConfiguration.Contexts;
using Microsoft.Extensions.Configuration;

//----------------------------- InOnConfiguringContext ---------------------------
using (InOnConfiguringContext db = new InOnConfiguringContext())
{
    Console.WriteLine("InOnConfiguringContext_________________");
    var users = db.Users.ToList();
    foreach (User u in users)
        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
}

//----------------------------- InDbContextOptionsContext ---------------------------
var optionsBuilder = new DbContextOptionsBuilder<InDbContextOptionsContext>();
var options = optionsBuilder
        .UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=UsersCURD; Trusted_Connection=True;")
        .Options;

using (InDbContextOptionsContext db = new InDbContextOptionsContext(options))
{
    Console.WriteLine("InDbContextOptionsContext_________________");
    var users = db.Users.ToList();
    foreach (User u in users)
        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
}

//----------------------------- InJsonFileContext ---------------------------
var builder = new ConfigurationBuilder();
// установка пути к текущему каталогу
builder.SetBasePath(Directory.GetCurrentDirectory());
// получаем конфигурацию из файла appsettings.json
builder.AddJsonFile("appsettings.json");
// создаем конфигурацию
var config = builder.Build();
// получаем строку подключения
string connectionString = config.GetConnectionString("JsonConnection");

var optionsBuilder_ = new DbContextOptionsBuilder<InDbContextOptionsContext>();
var options_ = optionsBuilder_.UseSqlServer(connectionString).Options;
using (InDbContextOptionsContext db = new InDbContextOptionsContext(options_))
{
    Console.WriteLine("InJsonFileContext_________________");
    var users = db.Users.ToList();
    foreach (User u in users)
    {
        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
    }
}
Console.Read();