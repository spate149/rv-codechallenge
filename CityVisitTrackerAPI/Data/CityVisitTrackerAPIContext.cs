using Microsoft.EntityFrameworkCore;
using CityVisitTrackerAPI.Models;

namespace CityVisitTrackerAPI.Data
{
    public class CityVisitTrackerAPIContext : DbContext
    {
        public CityVisitTrackerAPIContext (DbContextOptions<CityVisitTrackerAPIContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCityVisit>().HasKey(c => new {c.CityId, c.UserId});
        }

        public DbSet<Models.State> State { get; set; }

        public DbSet<City> City { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<UserCityVisit> UserCityVisits { get; set; }
    }
}
