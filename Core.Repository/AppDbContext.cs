using Core.Modes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.Repository
{
    public class AppDbContext : IdentityDbContext
    {
        private IConfigurationRoot _config;
        public AppDbContext(DbContextOptions options, IConfigurationRoot config)
      : base(options)
        {
            _config = config;
        } 
        public DbSet<DataDictionary> DataDictionary { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["Data:ConnectionString"]); 
        }
    }
}
