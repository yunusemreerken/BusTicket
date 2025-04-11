using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusTicket.Models.Entities
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        public int VoyageId { get; set; } // Foreign Key

        [Required]
        public int SeatNumber { get; set; } // Hangi koltuk olduğu

        [Required]
        public int PassengerId { get; set; } // Foreign Key

        [Required]
        public DateTime BuyDate { get; set; } = DateTime.UtcNow; // Otomatik tarih

        [Required]
        [StringLength(20)]
        public string PNR { get; set; } = string.Empty; // Benzersiz Bilet Numarası

        // Navigation Properties
        [ForeignKey("VoyageId")]
        public virtual Voyage? Voyage { get; set; }

        [ForeignKey("PassengerId")]
        public virtual Passenger? Passenger { get; set; }
    }
}