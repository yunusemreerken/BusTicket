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
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Voyage> Voyages { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Önce base metodu çağırın (Identity tabloları için önemli)

            // Kendi model yapılandırmalarınız burada devam edebilir
            builder.Entity<Ticket>()
                .HasIndex(b => b.PNR)
                .IsUnique();

            builder.Entity<Seat>()
                .HasIndex(s => new { s.VoyageId, s.SeatNumber })
                .IsUnique();

            builder.Entity<Ticket>()
                .HasOne(b => b.Voyage)
                .WithMany(s => s.Tickets)
                .HasForeignKey(b => b.VoyageId)
                .OnDelete(DeleteBehavior.Restrict);

            // Identity tabloları için özel yapılandırmalar gerekiyorsa buraya eklenebilir
            // Örneğin, tablo isimlerini değiştirmek vb.
        }
    }
}