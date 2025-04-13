using Microsoft.AspNetCore.Mvc;

namespace BusTicket.Areas.Admin.Controllers;

[Area("Admin")] // <<<--- BU ÇOK ÖNEMLİ!
public class AdminController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}