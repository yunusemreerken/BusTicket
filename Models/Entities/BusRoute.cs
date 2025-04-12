namespace BusTicket.Models.Entities;

public class BusRoute
{
    public int Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    // Other relevant properties like distance, estimated duration, etc.
    public virtual ICollection<BusSchedule> Schedules { get; set; } // Navigation property
}