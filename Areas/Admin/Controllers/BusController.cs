using BusTicket.Data;
using Microsoft.AspNetCore.Mvc;

namespace BusTicket.Areas.Admin.Controllers;

[Area("Admin")]
public class BusController : Controller
{
    private readonly ApplicationDbContext _context;
    public BusController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }
}