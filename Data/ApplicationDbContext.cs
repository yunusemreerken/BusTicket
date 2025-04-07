using Microsoft.EntityFrameworkCore;
using BusTicket.Models.Entities; // Model sınıflarınızın namespace'i

namespace BusTicket.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Veritabanı tablolarına karşılık gelen DbSet'ler
        public DbSet<Otobus> Otobusler { get; set; }
        public DbSet<Guzergah> Guzergahlar { get; set; }
        public DbSet<Sefer> Seferler { get; set; }
        public DbSet<Koltuk> Koltuklar { get; set; }
        public DbSet<Yolcu> Yolcular { get; set; }
        public DbSet<Bilet> Biletler { get; set; }

        // (İsteğe bağlı) Model yapılandırmalarını burada veya ayrı Configuration sınıflarında yapabilirsiniz.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Örnek: Benzersiz PNR numarası sağlamak için Index ekleme
            modelBuilder.Entity<Bilet>()
                .HasIndex(b => b.PNR)
                .IsUnique();

            // Örnek: Bir seferdeki koltuk numarasının benzersiz olmasını sağlama
            modelBuilder.Entity<Koltuk>()
                .HasIndex(k => new { k.SeferID, k.KoltukNumarasi })
                .IsUnique();

            // İlişkileri daha detaylı yapılandırmak isterseniz burada Fluent API kullanabilirsiniz.
            // Örnek: Sefer ve Bilet arasındaki ilişki (genelde otomatik algılanır ama açıkça belirtilebilir)
            modelBuilder.Entity<Bilet>()
                .HasOne(b => b.Sefer)
                .WithMany(s => s.Biletler)
                .HasForeignKey(b => b.SeferID)
                .OnDelete(DeleteBehavior.Restrict); // Bir sefer silinirse ilişkili biletler ne olacak? (Restrict: Engelle)
        }
    }
}