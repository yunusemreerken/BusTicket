using System.ComponentModel.DataAnnotations;

namespace BusTicket.Models.Entities
{
    public class Bus
    {
        [Key] // Primary Key
        public int BusId { get; set; }

        [Required(ErrorMessage = "Plaka boş bırakılamaz.")]
        [StringLength(15)]
        public string Plate { get; set; } = string.Empty; // Boş olmaması için başlangıç değeri

        [Required(ErrorMessage = "Koltuk kapasitesi boş bırakılamaz.")]
        [Range(10, 60, ErrorMessage = "Kapasite 10 ile 60 arasında olmalıdır.")]
        public int SeatCapacity { get; set; }

        // İlişkili Seferler (Navigation Property)
        public virtual ICollection<Voyage> Voyages { get; set; } = new List<Voyage>();
    }
}