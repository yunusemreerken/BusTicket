using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusTicket.Models.Entities
{
    // Şehir Bilgileri
    public class City
    {
        public int Id { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Bir şehrin kalkış noktası olduğu güzergahlar
        public virtual IEnumerable<BusRoute>? OriginRoutes { get; set; } = new List<BusRoute>();

        // Bir şehrin varış noktası olduğu güzergahlar
        public virtual IEnumerable<BusRoute>? DestinationRoutes { get; set; } = new List<BusRoute>();
    }

    
}