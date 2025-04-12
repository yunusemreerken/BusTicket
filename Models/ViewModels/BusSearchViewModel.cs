using BusTicket.Models.Entities;

namespace BusTicket.Models.ViewModels;

public class BusSearchViewModel
{
    // Search Criteria
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public DateTime? DepartureDate { get; set; }

    // Results
    public List<BusSchedule>? Schedules { get; set; }

    // Optional: For dropdowns if you have predefined locations
    // public SelectList? Origins { get; set; }
    // public SelectList? Destinations { get; set; }
}