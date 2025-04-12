// using Microsoft.EntityFrameworkCore; // Bu satırı silin veya yorumlayın
using Microsoft.AspNetCore.Identity; // Bunu ekleyin
using Microsoft.EntityFrameworkCore; // Bu kalsın
using BusTicket.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BusTicket.Data
{
    // DbContext yerine IdentityDbContext'ten miras alıyoruz
    // Varsayılan IdentityUser sınıfını kullanacağız
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Kendi DbSet'leriniz burada kalmaya devam edecek
        public DbSet<BusRoute> BusRoutes { get; set; }
        public DbSet<BusSchedule> BusSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Önce base metodu çağırın (Identity tabloları için önemli)
           
            modelBuilder.Entity<BusSchedule>()
                .HasOne(s => s.Route)
                .WithMany(r => r.Schedules)
                .HasForeignKey(s => s.BusRouteId);
            // Kendi model yapılandırmalarınız burada devam edebilir
        }
    }
}