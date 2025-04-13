using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BusTicket.Models;

namespace BusTicket.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Filo()
    {
        return View();
    }
    
    public IActionResult Search()
    {
        return View();
    }

    // GET: /Home/About
    public IActionResult About()
    {
        ViewData["Title"] = "Hakkımızda"; // Sayfa başlığını ayarla
        return View(); // Views/Home/About.cshtml'i döndürür
    }

    // GET: /Home/Contacts
    public IActionResult Contacts()
    {
        ViewData["Title"] = "İletişim"; // Sayfa başlığını ayarla
        return View(); // Views/Home/Contacts.cshtml'i döndürür
    }

    // GET: /Home/Typography
    public IActionResult Typography()
    {
        ViewData["Title"] = "Tipografi"; // Sayfa başlığını ayarla
        return View(); // Views/Home/Typography.cshtml'i döndürür
    }

    // Projenizde varsa Error action'ı genellikle burada kalır
    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    
}