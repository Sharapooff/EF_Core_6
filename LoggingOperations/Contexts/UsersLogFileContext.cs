using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using LoggingOperations.Models;
using Microsoft.Extensions.Logging;

namespace LoggingOperations.Contexts
{
    internal class UsersLogFileContext : DbContext
    {        
        private readonly StreamWriter logStream = new StreamWriter("mylog.txt", true);
        public DbSet<User> Users => Set<User>(); //{ get; set; } = null!; //{ get; set; }
        public UsersLogFileContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=UsersCURD; Trusted_Connection=True;");
            optionsBuilder.LogTo(logStream.WriteLine);
            //Метод LogTo() имеет ряд перегруженных версий
            //можно применять различный уровень логирования
            //optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            //можно отфильтровать сообщения другим способом
            //категориями, которые представлены классом DbLoggerCategory
            //optionsBuilder.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name });
        }

        //Для закрытия и утилизации файлового потока StreamWriter в классе контекста переопределены методы
        //Dispose/DisposeAsync, в которых вызывается метод Dispose/DisposeAsync объекта StreamWriter.
        public override void Dispose()
        {
            base.Dispose();
            logStream.Dispose();
        }
        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await logStream.DisposeAsync();
        }
    }
}
