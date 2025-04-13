using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusTicket.Models.Entities;

// Bilet Durumu Enum
public enum TicketStatus
{
    Booked,     // Rezerve Edilmiş / Satılmış
    Cancelled,  // İptal Edilmiş
    Pending     // Beklemede (Opsiyonel)
}

// Bilet Bilgileri
public class Ticket
{
    public int Id { get; set; } // Primary Key

    [Required]
    public int TripId { get; set; } // Foreign Key (Hangi Sefere Ait)

    [Required]
    [StringLength(100)]
    public string PassengerName { get; set; } // Yolcu Adı Soyadı

    [Required]
    [StringLength(15)]
    public string PassengerPhone { get; set; } // Yolcu Telefon

    [EmailAddress]
    [StringLength(100)]
    public string? PassengerEmail { get; set; } // Yolcu E-posta (Opsiyonel)

    [Required]
    public int SeatNumber { get; set; } // Koltuk Numarası

    [Required]
    public DateTime BookingDate { get; set; } // Biletin Kesildiği Tarih

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePaid { get; set; } // Ödenen Ücret

    [Required]
    public TicketStatus Status { get; set; } // Bilet Durumu (Booked, Cancelled)

    // Navigation Property
    [ForeignKey("TripId")]
    public virtual Trip Trip { get; set; }
}