using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusTicket.Models.Entities
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }

        [Required]
        public int VoyageId { get; set; } // Foreign Key

        [Required]
        public int SeatNumber { get; set; }

        [Required]
        public bool Full { get; set; } = false; // Başlangıçta boş

        // Navigation Property
        [ForeignKey("VoyageId")]
        public virtual Voyage? Voyage { get; set; }
    }
}