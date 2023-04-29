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
            optionsBuilder.UseSqlServer("Server=tcp:tn-server.database.windows.net,1433;Initial Catalog=tn-db;Persist Security Info=False;User ID=tomanota-admin;Password=SqYHE8Y9k3aGPRG;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
		}

        public DbSet<Prompt> Prompts { get; set; }
        public DbSet<User> Users { get; set; }
		public DbSet<PromptBody> PromptBodies { get; set; }
		public DbSet<WeekProgress> WeekProgresses { get; set; }
		public DbSet<YearProgress> YearProgresses { get; set; }
		public DbSet<CompletedPremadeTitle> CompletedPremadeTitles { get; set; }
	}
}
