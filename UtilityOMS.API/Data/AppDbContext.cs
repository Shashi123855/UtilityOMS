using Microsoft.EntityFrameworkCore;
using UtilityOMS.API.Models;

namespace UtilityOMS.API.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base (options)
        {
            
        }
        // Tables 
        public DbSet<Outage> Outages { get; set; }
        public DbSet<Resident> Residents { get; set; }
        public DbSet<Crew>  Crews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Default  Crews
            modelBuilder.Entity<Crew>().HasData(
                 new Crew
                 {
                     Id = 1,
                     Name="Alpha Team",
                     LeadTechnician ="John Smith",
                     PhoneNumber = "555-1001",
                     Status= CrewStatus.Available
                 },
                new Crew
                {
                    Id = 2,
                    Name = "Beta Team",
                    LeadTechnician = "Jane Doe",
                    PhoneNumber = "555-1002",
                    Status = CrewStatus.Available
                },
                new Crew
                {
                    Id = 3,
                    Name = "Delta Team",
                    LeadTechnician = "Carlos Rivera",
                    PhoneNumber = "555-1003",
                    Status = CrewStatus.OffDuty
                }
            );
            // Seed a sample resident
            modelBuilder.Entity<Resident>().HasData(
                new Resident
                {
                    Id = 1,
                    Name = "Alice Johnson",
                    PhoneNumber = "555-2001",
                    Email = "alice@example.com",
                    Address = "123 Main St",
                    Latitude = 40.7128,
                    Longitude = -74.0060,
                    SmsOptIn = true,
                    EmailOptIn = true
                }
            );

            modelBuilder.Entity<Outage>()
                 .Property(o => o.ReportedAt)
                 .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Resident>()
                .Property(r => r.RegisteredAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
