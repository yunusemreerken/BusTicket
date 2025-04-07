using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusTicket.Models.Entities
{
    public class Bilet
    {
        [Key]
        public int BiletID { get; set; }

        [Required]
        public int SeferID { get; set; } // Foreign Key

        [Required]
        public int KoltukNumarasi { get; set; } // Hangi koltuk olduğu

        [Required]
        public int YolcuID { get; set; } // Foreign Key

        [Required]
        public DateTime SatinAlmaTarihi { get; set; } = DateTime.UtcNow; // Otomatik tarih

        [Required]
        [StringLength(20)]
        public string PNR { get; set; } = string.Empty; // Benzersiz Bilet Numarası

        // Navigation Properties
        [ForeignKey("SeferID")]
        public virtual Sefer? Sefer { get; set; }

        [ForeignKey("YolcuID")]
        public virtual Yolcu? Yolcu { get; set; }
    }
}