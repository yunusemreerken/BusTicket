using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusTicket.Models.Entities
{
    public class Sefer
    {
        [Key]
        public int SeferID { get; set; }

        [Required]
        public int GuzergahID { get; set; } // Foreign Key

        [Required]
        public int OtobusID { get; set; } // Foreign Key

        [Required(ErrorMessage = "Kalkış zamanı boş bırakılamaz.")]
        public DateTime KalkisZamani { get; set; }

        [Required(ErrorMessage = "Varış zamanı boş bırakılamaz.")]
        public DateTime VarisZamani { get; set; }

        [Required(ErrorMessage = "Bilet fiyatı boş bırakılamaz.")]
        [Column(TypeName = "decimal(18,2)")] // Veritabanında para birimi için uygun tip
        [Range(1, 10000, ErrorMessage = "Fiyat 1 ile 10000 arasında olmalıdır.")]
        public decimal BiletFiyati { get; set; }

        // Navigation Properties
        [ForeignKey("GuzergahID")]
        public virtual Guzergah? Guzergah { get; set; } // ? ile nullable yapıldı

        [ForeignKey("OtobusID")]
        public virtual Otobus? Otobus { get; set; } // ? ile nullable yapıldı

        public virtual ICollection<Koltuk> Koltuklar { get; set; } = new List<Koltuk>();
        public virtual ICollection<Bilet> Biletler { get; set; } = new List<Bilet>();
    }
}