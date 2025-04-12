namespace BusTicket.Models.Entities;

public class BusSchedule
{
    public int Id { get; set; }
    public int BusRouteId { get; set; } // Foreign key
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public decimal Price { get; set; }
    public int AvailableSeats { get; set; }
    // Other properties like bus type, amenities, etc.
    public virtual BusRoute Route { get; set; } // Navigation property
}