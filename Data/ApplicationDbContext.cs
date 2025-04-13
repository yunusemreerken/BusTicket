// using Microsoft.EntityFrameworkCore; // Bu satırı silin veya yorumlayın

using BusTicket.Models.Entities;
using Microsoft.AspNetCore.Identity; // Bunu ekleyin
using Microsoft.EntityFrameworkCore; // Bu kalsın
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BusTicket.Models.Entities
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
        public DbSet<City> Cities { get; set; }
        public DbSet<BusRoute> BusRoutes { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Önce base metodu çağırın (Identity tabloları için önemli)


            // Örnek: İlişki yapılandırması
            modelBuilder.Entity<BusRoute>() // <<<--- "BusRoute" mu?
                .HasOne(r => r.OriginCity)
                .WithMany(c => c.OriginRoutes)
                .HasForeignKey(r => r.OriginCityId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<BusRoute>() // <<<--- "BusRoute" mu?
                .HasOne(r => r.DestinationCity)
                .WithMany(c => c.DestinationRoutes)
                .HasForeignKey(r => r.DestinationCityId)
                .OnDelete(DeleteBehavior.Restrict); 

            // Otobüs Plakasının benzersiz (unique) olmasını sağlama
            modelBuilder.Entity<Bus>()
                .HasIndex(b => b.PlateNumber)
                .IsUnique();

            // Bir seferdeki koltuk numarasının benzersiz olmasını sağlama (TripId + SeatNumber)
            modelBuilder.Entity<Ticket>()
                .HasIndex(t => new { t.TripId, t.SeatNumber })
                .IsUnique();

            // Ondalık (decimal) tipler için hassasiyet belirleme (SQL Server için)
            modelBuilder.Entity<Trip>()
                .Property(t => t.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Ticket>()
                .Property(t => t.PricePaid)
                .HasPrecision(18, 2);

            // Enum'ın veritabanında string olarak saklanmasını sağlama (isteğe bağlı, okunabilirlik için)
            modelBuilder.Entity<Ticket>()
                .Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20); // Enum string değerinin max uzunluğu

            // Başlangıç Verisi (Seed Data) Eklemek İsterseniz:
            // modelBuilder.Entity<City>().HasData(
            //     new City { Id = 1, Name = "İstanbul" },
            //     new City { Id = 2, Name = "Ankara" }
            // );
        }
    }
}