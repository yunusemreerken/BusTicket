using System.ComponentModel.DataAnnotations;

namespace BusTicket.Models.Entities
{
    public class Passenger
    {
        [Key]
        public int PassengerId { get; set; }

        [Required(ErrorMessage = "Ad boş bırakılamaz.")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad boş bırakılamaz.")]
        [StringLength(50)]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [StringLength(15)]
        public string? Phone { get; set; } // Opsiyonel olabilir

        // İlişkili Biletler
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}