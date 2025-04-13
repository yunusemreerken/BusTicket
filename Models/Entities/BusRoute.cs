using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusTicket.Models.Entities;

public class BusRoute
{
    public int Id { get; set; } // Primary Key

    [Required]
    public int OriginCityId { get; set; } // Foreign Key (Kalkış Şehri)

    [Required]
    public int DestinationCityId { get; set; } // Foreign Key (Varış Şehri)

    [StringLength(200)]
    public string? Description { get; set; } // Opsiyonel: "Express", "Normal" vb.

    // Navigation Properties
    [ForeignKey("OriginCityId")]
    public virtual City OriginCity { get; set; }

    [ForeignKey("DestinationCityId")]
    public virtual City DestinationCity { get; set; }

    // Bu güzergaha ait seferler
    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}