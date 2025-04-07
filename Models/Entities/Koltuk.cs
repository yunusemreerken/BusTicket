using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusTicket.Models.Entities
{
    public class Koltuk
    {
        [Key]
        public int KoltukID { get; set; }

        [Required]
        public int SeferID { get; set; } // Foreign Key

        [Required]
        public int KoltukNumarasi { get; set; }

        [Required]
        public bool DoluMu { get; set; } = false; // Başlangıçta boş

        // Navigation Property
        [ForeignKey("SeferID")]
        public virtual Sefer? Sefer { get; set; }
    }
}