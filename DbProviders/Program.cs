using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using DbProviders.Models;
using DbProviders.Contexts;
using Microsoft.Extensions.Configuration;

//----------------------------- SqlServer ---------------------------
var builder_SqlServer = new ConfigurationBuilder();
// установка пути к текущему каталогу
builder_SqlServer.SetBasePath(Directory.GetCurrentDirectory());
// получаем конфигурацию из файла appsettings.json
builder_SqlServer.AddJsonFile("appsettings.json");
// создаем конфигурацию
var config = builder_SqlServer.Build();
// получаем строку подключения
string connectionString_SqlServer = config.GetConnectionString("SqlServer");

var optionsBuilder_SqlServer = new DbContextOptionsBuilder<SqlServerUserContext>();
var options_SqlServer = optionsBuilder_SqlServer.UseSqlServer(connectionString_SqlServer).Options;
using (SqlServerUserContext db = new SqlServerUserContext(options_SqlServer))
{
    Console.WriteLine("_SqlServer_________________");
    var users = db.Users.ToList();
    foreach (User u in users)
    {
        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
    }
}
//----------------------------- MySql ---------------------------
//  MySql не установлен!
//var builder_MySql = new ConfigurationBuilder();
//// установка пути к текущему каталогу
//builder_MySql.SetBasePath(Directory.GetCurrentDirectory());
//// получаем конфигурацию из файла appsettings.json
//builder_MySql.AddJsonFile("appsettings.json");
//// создаем конфигурацию
//var config_MySql = builder_MySql.Build();
//// получаем строку подключения
//string connectionString_MySql = config.GetConnectionString("MySql");

//var optionsBuilder_MySql = new DbContextOptionsBuilder<SqlServerUserContext>();
//var options_MySql = optionsBuilder_MySql.UseMySql(connectionString_MySql, new MySqlServerVersion(new Version(8, 0, 25))).Options;
//using (SqlServerUserContext db = new SqlServerUserContext(options_MySql))
//{
//    Console.WriteLine("_MySql_________________");
//    var users = db.Users.ToList();
//    foreach (User u in users)
//    {
//        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
//    }
//}
//----------------------------- Npgsql ---------------------------
var builder_Npgsql = new ConfigurationBuilder();
// установка пути к текущему каталогу
builder_Npgsql.SetBasePath(Directory.GetCurrentDirectory());
// получаем конфигурацию из файла appsettings.json
builder_Npgsql.AddJsonFile("appsettings.json");
// создаем конфигурацию
var config_Npgsql = builder_Npgsql.Build();
// получаем строку подключения
string connectionString_Npgsql = config_Npgsql.GetConnectionString("Npgsql");

var optionsBuilder_Npgsql = new DbContextOptionsBuilder<NpgsqlUserContext>();
var options_Npgsql = optionsBuilder_Npgsql.UseNpgsql(connectionString_Npgsql).Options;
using (NpgsqlUserContext db = new NpgsqlUserContext(options_Npgsql))
{
    Console.WriteLine("_Npgsql_________________");
    var users = db.Users.ToList();
    foreach (User u in users)
    {
        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
    }
}
Console.Read();