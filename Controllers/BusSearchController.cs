using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicket.Data; // Replace with your DbContext namespace
using BusTicket.Models.Entities; // Replace with your Models namespace
using System.Linq;
using System.Threading.Tasks;
using BusTicket.Models.ViewModels;

public class BusSearchController : Controller
{
    private readonly ApplicationDbContext _context;

    public BusSearchController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /BusSearch/ or /BusSearch/Index
    // Accepts search parameters from the query string
    public async Task<IActionResult> Index(string origin, string destination, DateTime? departureDate)
    {
        // Start with a base query including the related Route data
        var schedulesQuery = _context.BusSchedules
                                     .Include(s => s.Route) // Eager load route details
                                     .Select(s => s); // Project to BusSchedule

        bool isSearchPerformed = false;

        // Apply filters based on input
        if (!string.IsNullOrEmpty(origin))
        {
            schedulesQuery = schedulesQuery.Where(s => s.Route.Origin.Contains(origin));
            isSearchPerformed = true;
        }

        if (!string.IsNullOrEmpty(destination))
        {
            schedulesQuery = schedulesQuery.Where(s => s.Route.Destination.Contains(destination));
            isSearchPerformed = true;
        }

        if (departureDate.HasValue)
        {
            // Compare only the Date part
            schedulesQuery = schedulesQuery.Where(s => s.DepartureTime.Date == departureDate.Value.Date);
            isSearchPerformed = true;
        }

        // Create the ViewModel
        var viewModel = new BusSearchViewModel
        {
            Origin = origin,
            Destination = destination,
            DepartureDate = departureDate,
            // Only execute the query and fetch results if a search was actually performed
            // Or decide if you want to show all schedules by default if no criteria given
            Schedules = isSearchPerformed ? await schedulesQuery.ToListAsync() : new List<BusSchedule>()
            // Optionally populate SelectLists for dropdowns here if needed
        };

        return View(viewModel); // Pass the ViewModel to the view
    }
}