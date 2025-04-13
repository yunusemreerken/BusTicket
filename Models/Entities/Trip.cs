using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusTicket.Models.Entities;

// Sefer (Belirli bir güzergahta, belirli bir otobüsle, belirli bir zamanda yapılan yolculuk)
public class Trip
{
    public int Id { get; set; } // Primary Key

    [Required]
    public int RouteId { get; set; } // Foreign Key (Hangi Güzergah)

    [Required]
    public int BusId { get; set; } // Foreign Key (Hangi Otobüs)

    [Required]
    public DateTime DepartureTime { get; set; } // Kalkış Zamanı

    [Required]
    public DateTime ArrivalTime { get; set; } // Tahmini Varış Zamanı

    [Required]
    [Column(TypeName = "decimal(18,2)")] // Para birimi için ondalık tip
    public decimal Price { get; set; } // Bilet Fiyatı

    // Navigation Properties
    [ForeignKey("RouteId")]
    public virtual Route Route { get; set; }

    [ForeignKey("BusId")]
    public virtual Bus Bus { get; set; }

    // Bu sefere ait biletler
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}