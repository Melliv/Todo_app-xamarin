using System.IO;
using Domain;
using Microsoft.EntityFrameworkCore;
using Xamarin.Essentials;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Priority> Priorities { get; set; }

        public AppDbContext()
        {
            SQLitePCL.Batteries_V2.Init();
            Database.EnsureCreated();
        }

        public void DeleteDb()
        {
            Database.EnsureDeleted();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var dbLocation = Path.Combine(FileSystem.AppDataDirectory, "ap.db");
            optionsBuilder.UseSqlite($"filename={dbLocation}");
        }
    }
}