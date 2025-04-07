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
        public DbSet<Otobus> Otobusler { get; set; }
        public DbSet<Guzergah> Guzergahlar { get; set; }
        public DbSet<Sefer> Seferler { get; set; }
        public DbSet<Koltuk> Koltuklar { get; set; }
        public DbSet<Yolcu> Yolcular { get; set; }
        public DbSet<Bilet> Biletler { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Önce base metodu çağırın (Identity tabloları için önemli)

            // Kendi model yapılandırmalarınız burada devam edebilir
            builder.Entity<Bilet>()
                .HasIndex(b => b.PNR)
                .IsUnique();

            builder.Entity<Koltuk>()
                .HasIndex(k => new { k.SeferID, k.KoltukNumarasi })
                .IsUnique();

            builder.Entity<Bilet>()
                .HasOne(b => b.Sefer)
                .WithMany(s => s.Biletler)
                .HasForeignKey(b => b.SeferID)
                .OnDelete(DeleteBehavior.Restrict);

            // Identity tabloları için özel yapılandırmalar gerekiyorsa buraya eklenebilir
            // Örneğin, tablo isimlerini değiştirmek vb.
        }
    }
}