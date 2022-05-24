
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using FluentAPI_Annotations.Models;
using FluentAPI_Annotations.Contexts;
using Microsoft.Extensions.Configuration;
/* Fluent API по большому счету представляет набор методов, которые определяются сопоставление между 
 * классами и их свойствами и таблицами и их столбцами. Как правило, функционал Fluent API задействуется
 * при переопределении метода OnModelCreating (в контексте + примеры).
 * 
 * Аннотации представляют настройку сопоставления моделей и таблиц с помощью атрибутов. Большинство классов
 * аннотаций располагаются в пространстве System.ComponentModel.DataAnnotations (в описании модели)
 *  
 */

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

var optionsBuilder_SqlServer = new DbContextOptionsBuilder<SqlServerPhonesContext>();
var options_SqlServer = optionsBuilder_SqlServer.UseSqlServer(connectionString_SqlServer).Options;
using (SqlServerPhonesContext db = new SqlServerPhonesContext(options_SqlServer))
{
    Console.WriteLine("_SqlServer_________________");
    var phones = db.Phones.ToList();
    foreach (Phone p in phones)
    {
        Console.WriteLine($"{p.Id}:{p.Name} - {p.Price}_{p.Discount}");
    }
}

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

var optionsBuilder_Npgsql = new DbContextOptionsBuilder<NpgsqlPhonesContext>();
var options_Npgsql = optionsBuilder_Npgsql.UseNpgsql(connectionString_Npgsql).Options;
using (NpgsqlPhonesContext db = new NpgsqlPhonesContext(options_Npgsql))
{
    Console.WriteLine("_Npgsql_________________");
    var phones = db.Phones.ToList();
    foreach (Phone p in phones)
    {
        Console.WriteLine($"{p.Id}:{p.Name} - {p.Price}_{p.Discount}");
    }
}
Console.Read();