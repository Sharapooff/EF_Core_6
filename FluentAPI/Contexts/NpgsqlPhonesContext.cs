
using Microsoft.EntityFrameworkCore;
using FluentAPI_Annotations.Models;

namespace FluentAPI_Annotations.Contexts
{
    internal class NpgsqlPhonesContext : DbContext
    {
        public DbSet<Phone> Phones => Set<Phone>(); //{ get; set; } = null!; //{ get; set; }        
        public NpgsqlPhonesContext(DbContextOptions<NpgsqlPhonesContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // использование Fluent API
            base.OnModelCreating(modelBuilder);
        }
    }
}
