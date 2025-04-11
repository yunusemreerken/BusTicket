using System.ComponentModel.DataAnnotations;

namespace BusTicket.Models.Entities
{
    public class Course
    {
        [Key]
        public int RouteId { get; set; }

        [Required(ErrorMessage = "Kalkış şehri boş bırakılamaz.")] [StringLength(50)]
        public string TakeOffCity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Varış şehri boş bırakılamaz.")]
        [StringLength(50)]
        public string ArrivalCity { get; set; } = string.Empty;

        // İlişkili Seferler
        public virtual ICollection<Voyage> Voyage { get; set; } = new List<Voyage>();
    }
}