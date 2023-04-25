global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Collections.Generic;

namespace user_web_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=tcp:tomanota-server.database.windows.net,1433;Initial Catalog=tomanota-db;Persist Security Info=False;User ID=tomanota-admin;Password=SqYHE8Y9k3aGPRG;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public DbSet<userNotes> userNotes { get; set; }
        public DbSet<User> users { get; set; }
    }
}
