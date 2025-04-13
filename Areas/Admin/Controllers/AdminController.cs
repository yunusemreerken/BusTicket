using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusTicket.Areas.Admin.Controllers;

[Area("Admin")] // <<<--- BU ÇOK ÖNEMLİ!
[Authorize]     // Bu controller'a sadece giriş yapmış kullanıcılar erişebilir 

public class AdminController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}