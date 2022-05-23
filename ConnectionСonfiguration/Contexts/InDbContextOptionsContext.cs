using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ConnectionConfiguration.Models;

namespace ConnectionConfiguration.Contexts
{
    internal class InDbContextOptionsContext : DbContext
    {
        public DbSet<User> Users => Set<User>(); //{ get; set; } = null!; //{ get; set; }

        //передача в конструктор базового класса объекта DbContextOptions, который инкапсулирует параметры конфигурации
        public InDbContextOptionsContext(DbContextOptions<InDbContextOptionsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
