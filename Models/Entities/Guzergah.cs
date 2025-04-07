using System.ComponentModel.DataAnnotations;

namespace BusTicket.Models.Entities
{
    public class Guzergah
    {
        [Key]
        public int GuzergahID { get; set; }

        [Required(ErrorMessage = "Kalkış şehri boş bırakılamaz.")]
        [StringLength(50)]
        public string KalkisSehri { get; set; } = string.Empty;

        [Required(ErrorMessage = "Varış şehri boş bırakılamaz.")]
        [StringLength(50)]
        public string VarisSehri { get; set; } = string.Empty;

        // İlişkili Seferler
        public virtual ICollection<Sefer> Seferler { get; set; } = new List<Sefer>();
    }
}