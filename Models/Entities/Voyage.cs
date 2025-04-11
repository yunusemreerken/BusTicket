using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusTicket.Models.Entities
{
    public class Voyage
    {
        [Key]
        public int VoyageId { get; set; }

        [Required]
        public int RouteId { get; set; } // Foreign Key

        [Required]
        public int BusId { get; set; } // Foreign Key

        [Required(ErrorMessage = "Kalkış zamanı boş bırakılamaz.")]
        public DateTime TakeOff { get; set; }

        [Required(ErrorMessage = "Varış zamanı boş bırakılamaz.")]
        public DateTime Arrival { get; set; }

        [Required(ErrorMessage = "Bilet fiyatı boş bırakılamaz.")]
        [Column(TypeName = "decimal(18,2)")] // Veritabanında para birimi için uygun tip
        [Range(1, 10000, ErrorMessage = "Fiyat 1 ile 10000 arasında olmalıdır.")]
        public decimal TicketPrice { get; set; }

        // Navigation Properties
        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; } // ? ile nullable yapıldı

        [ForeignKey("BusId")]
        public virtual Bus? Bus { get; set; } // ? ile nullable yapıldı

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}