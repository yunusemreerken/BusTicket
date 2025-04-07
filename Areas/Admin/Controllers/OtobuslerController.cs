using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicket.Data;
using BusTicket.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BusTicket.Areas.Admin.Controllers
{
    [Area("Admin")] // <-- Bu attribute'u ekle
    [Authorize(Roles = "Admin")] // <-- GÜVENLİK İÇİN DAHA SONRA EKLEYİN!
    public class OtobuslerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OtobuslerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Otobusler (Listeleme Sayfası)
        public async Task<IActionResult> Index()
        {
            // Tüm otobüsleri veritabanından çekip View'e gönderiyoruz
            return View(await _context.Otobusler.ToListAsync());
        }

        // GET: Otobusler/Details/5 (Detay Gösterme Sayfası)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Veya özel bir hata sayfasına yönlendir
            }

            var otobus = await _context.Otobusler
                .FirstOrDefaultAsync(m => m.OtobusID == id);
            if (otobus == null)
            {
                return NotFound(); // Veya özel bir hata sayfasına yönlendir
            }

            // Bulunan otobüsü View'e gönder
            return View(otobus);
        }

        // GET: Otobusler/Create (Yeni Otobüs Ekleme Formu)
        public IActionResult Create()
        {
            // Boş formu göster
            return View();
        }

        // POST: Otobusler/Create (Formdan Gelen Veriyi İşleme)
        // Cross-Site Request Forgery (CSRF) saldırılarını önlemek için ValidateAntiForgeryToken
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Plaka,KoltukKapasitesi")] Otobus otobus)
        {
            // ModelState.IsValid, modeldeki [Required], [StringLength] gibi kuralların
            // ve veri tiplerinin geçerli olup olmadığını kontrol eder.
            if (ModelState.IsValid)
            {
                _context.Add(otobus); // Yeni otobüsü contexte ekle
                await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
                return RedirectToAction(nameof(Index)); // Başarılı olursa Index sayfasına yönlendir
            }
            // Model geçerli değilse, formu hatalarla birlikte tekrar göster
            return View(otobus);
        }

        // GET: Otobusler/Edit/5 (Düzenleme Formu)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otobus = await _context.Otobusler.FindAsync(id);
            if (otobus == null)
            {
                return NotFound();
            }
            // Bulunan otobüs bilgileriyle dolu formu göster
            return View(otobus);
        }

        // POST: Otobusler/Edit/5 (Düzenleme Formundan Gelen Veriyi İşleme)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OtobusID,Plaka,KoltukKapasitesi")] Otobus otobus)
        {
            // Gelen ID ile modeldeki ID eşleşmiyorsa güvenlik riski olabilir
            if (id != otobus.OtobusID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(otobus); // Otobüsü güncelle
                    await _context.SaveChangesAsync(); // Değişiklikleri kaydet
                }
                catch (DbUpdateConcurrencyException) // Eş zamanlı güncelleme çakışması olursa
                {
                    if (!OtobusExists(otobus.OtobusID)) // Otobüs gerçekten silinmiş mi kontrol et
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Diğer concurrency hatalarını tekrar fırlat
                    }
                }
                return RedirectToAction(nameof(Index)); // Başarılı olursa Index'e yönlendir
            }
            // Model geçerli değilse, formu hatalarla tekrar göster
            return View(otobus);
        }

        // GET: Otobusler/Delete/5 (Silme Onay Sayfası)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otobus = await _context.Otobusler
                .FirstOrDefaultAsync(m => m.OtobusID == id);
            if (otobus == null)
            {
                return NotFound();
            }

            // Silinecek otobüs bilgilerini onay için View'e gönder
            return View(otobus);
        }

        // POST: Otobusler/Delete/5 (Silme İşlemini Gerçekleştirme)
        [HttpPost, ActionName("Delete")] // Formdan Delete action'ı çağrıldığında bu metoda gelir
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var otobus = await _context.Otobusler.FindAsync(id);
            if (otobus != null) // Otobüs bulunduysa sil
            {
                // ÖNEMLİ NOT: Eğer bu otobüsle ilişkili Seferler varsa, doğrudan silme işlemi
                // veritabanı kısıtlamaları nedeniyle hata verebilir (Foreign Key Constraint).
                // Gerçek bir uygulamada, önce ilişkili seferlerin durumunu kontrol etmek
                // veya farklı bir silme stratejisi (örn: "Pasif" olarak işaretleme) uygulamak gerekebilir.
                try
                {
                     _context.Otobusler.Remove(otobus);
                     await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                     // Hata yönetimi: Kullanıcıya bilgi verilebilir.
                     // Örneğin: "Bu otobüs aktif seferlerde kullanıldığı için silinemez."
                     // Şimdilik basit tutuyoruz ve hatayı loglayabiliriz.
                     ModelState.AddModelError("", "Bu otobüs silinemedi. İlişkili seferleri olabilir.");
                     // Hatayla birlikte onay sayfasını tekrar gösterelim
                     return View("Delete", otobus);
                }

            }
            return RedirectToAction(nameof(Index)); // Index'e yönlendir
        }

        // Yardımcı metot: Belirtilen ID'li otobüsün var olup olmadığını kontrol eder
        private bool OtobusExists(int id)
        {
            return _context.Otobusler.Any(e => e.OtobusID == id);
        }
    }
}