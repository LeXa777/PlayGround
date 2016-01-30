namespace TheWorld.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Data.Entity;

    public class WorldContext : IdentityDbContext<WorldUser>
    {

        public WorldContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = Startup.Configuration["Data:WorldContectConnection"];
            optionsBuilder.UseSqlServer(connString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
