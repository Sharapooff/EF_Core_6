using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MigrationApplication.Contexts;

namespace MigrationApplication.Classes
{
    internal class SampleContextFactory : IDesignTimeDbContextFactory<InDbContextOptionsContext>
    {
        public InDbContextOptionsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<InDbContextOptionsContext>();

            // получаем конфигурацию из файла appsettings.json
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();

            // получаем строку подключения из файла appsettings.json
            string connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
            return new InDbContextOptionsContext(optionsBuilder.Options);
        }
    }
}
