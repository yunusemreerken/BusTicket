using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicket.Data; // DbContext'in bulunduğu namespace
using BusTicket.Models.Entities; // Modellerin bulunduğu namespace
using System.Linq; // LINQ sorguları için
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Asenkron işlemler için

namespace BusTicket.Areas.Admin.Controllers
{
    [Area("Admin")] // <-- Bu attribute'u ekle
    [Authorize(Roles = "Admin")] // <-- GÜVENLİK İÇİN DAHA SONRA EKLEYİN!
    // [ApiController] ve ControllerBase yerine Controller sınıfından miras alıyoruz
    public class SeferlerController : Controller
    {
        private readonly ApplicationDbContext _context;

        // DbContext'i dependency injection ile alıyoruz
        public SeferlerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Seferler veya /Seferler/Index
        // Sefer arama formunu gösterir ve arama sonuçlarını listeler
        public async Task<IActionResult> Index(string kalkisSehri, string varisSehri)
        {
            // Başlangıçta tüm seferleri getirmek yerine sadece arama yapıldığında
            // sonuçları getirelim veya boş bir liste döndürelim.
            var seferler = new List<Sefer>();

            ViewData["KalkisSehri"] = kalkisSehri; // Arama formunda önceki değerleri tutmak için
            ViewData["VarisSehri"] = varisSehri;   // Arama formunda önceki değerleri tutmak için

            if (!string.IsNullOrEmpty(kalkisSehri) && !string.IsNullOrEmpty(varisSehri))
            {
                seferler = await _context.Seferler
                                        .Include(s => s.Guzergah) // İlişkili Güzergah bilgisini yükle
                                        .Include(s => s.Otobus)   // İlişkili Otobüs bilgisini yükle
                                        .Where(s => s.Guzergah != null &&
                                                    s.Guzergah.KalkisSehri.Contains(kalkisSehri) && // Benzerlik araması
                                                    s.Guzergah.VarisSehri.Contains(varisSehri) &&   // Benzerlik araması
                                                    s.KalkisZamani > DateTime.Now) // Sadece gelecek seferler
                                        .OrderBy(s => s.KalkisZamani)
                                        .ToListAsync();
            }
            // else durumunda boş seferler listesi View'e gönderilecek

            // seferler listesini (boş veya dolu) Index.cshtml View'ına gönderiyoruz
            return View(seferler);
        }

        // GET: /Seferler/Detay/5
        // Belirli bir seferin detaylarını gösterir
        public async Task<IActionResult> Detay(int? id) // id'nin null olabileceğini belirtiyoruz
        {
            if (id == null)
            {
                // ID gelmediyse NotFound (404) döndür
                // return NotFound(); // Standart 404 sayfası
                return View("NotFound"); // Özel bir NotFound view'ı gösterebiliriz
            }

            // Seferi, ilişkili bilgileri (Güzergah, Otobüs) ve koltukları ile birlikte getiriyoruz
            var sefer = await _context.Seferler
                                    .Include(s => s.Guzergah)
                                    .Include(s => s.Otobus)
                                    .Include(s => s.Koltuklar) // Koltuk bilgilerini de yüklüyoruz
                                    .FirstOrDefaultAsync(m => m.SeferID == id);

            if (sefer == null)
            {
                // Belirtilen ID ile sefer bulunamadıysa NotFound (404) döndür
                // return NotFound();
                 return View("NotFound"); // Özel bir NotFound view'ı gösterebiliriz
            }

            // Bulunan sefer nesnesini Detay.cshtml View'ına gönderiyoruz
            return View(sefer);
        }

        // Buraya bilet alma, sefer ekleme/düzenleme/silme gibi
        // diğer MVC action'ları da ekleyebilirsiniz.
        // Örn: Bilet Alma Sayfası (GET)
        // public IActionResult BiletAl(int seferId, int koltukNo) { ... return View(viewModel); }

        // Örn: Bilet Alma İşlemi (POST)
        // [HttpPost]
        // public async Task<IActionResult> BiletAlPost(BiletAlViewModel model) { ... return RedirectToAction("Basarili"); }
    }
}