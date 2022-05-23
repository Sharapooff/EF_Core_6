using LoggingOperations.Contexts;
using LoggingOperations.Models;
using LoggingOperations.Classes;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;

Console.WriteLine("ConsoleLog__________:");
using (UsersLogConsoleContext db = new UsersLogConsoleContext())
{
    var users = db.Users.ToList();    
    foreach (User u in users)
    {
        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
    }
}
Console.WriteLine("FileLog__________:");
using (UsersLogFileContext db = new UsersLogFileContext())
{
    var users = db.Users.ToList();
    foreach (User u in users)
    {
        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
    }
}
Console.WriteLine("MyLog__________Локальное применение провайдера:");
using (UsersMyLogLocalContext db = new UsersMyLogLocalContext())
{
    db.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
    var users = db.Users.ToList();
    Console.WriteLine("Данные после добавления:");
    foreach (User u in users)
    {
        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
    }
}
Console.WriteLine("MyLog__________Глобальное применение провайдера:");

Console.Read();