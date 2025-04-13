using System.ComponentModel.DataAnnotations;

namespace BusTicket.Models.Entities;

// Otobüs Bilgileri
public class Bus
{
    public int Id { get; set; } // Primary Key

    [Required]
    [StringLength(20)]
    public string PlateNumber { get; set; } // Plaka

    [StringLength(100)]
    public string? Model { get; set; } // Otobüs Modeli (Opsiyonel)

    [Required]
    public int Capacity { get; set; } // Koltuk Kapasitesi

    // Bu otobüsün yaptığı seferler
    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}