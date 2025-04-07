using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicket.Data;
using BusTicket.Models.Entities;

namespace BusTicket.Controllers
{
    public class SeferlerController : Controller
    {
        private readonly ApplicationDbContext _context; // DbContext'i enjekte et

        public SeferlerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/seferler
        // Kalkış ve varış şehrine göre seferleri listeleme
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sefer>>> GetSeferler([FromQuery] string kalkis, [FromQuery] string varis)
        {
            if (string.IsNullOrWhiteSpace(kalkis) || string.IsNullOrWhiteSpace(varis))
            {
                return BadRequest("Kalkış ve varış şehirleri belirtilmelidir.");
            }

            // Guzergah bilgisini ve Otobus bilgisini de dahil et (Include)
            var seferler = await _context.Seferler
                                        .Include(s => s.Guzergah)
                                        .Include(s => s.Otobus)
                                        .Where(s => s.Guzergah != null &&
                                                    s.Guzergah.KalkisSehri == kalkis &&
                                                    s.Guzergah.VarisSehri == varis &&
                                                    s.KalkisZamani > DateTime.UtcNow) // Sadece gelecek seferler
                                        .OrderBy(s => s.KalkisZamani)
                                        .ToListAsync();

            if (!seferler.Any())
            {
                return NotFound("Belirtilen kriterlere uygun sefer bulunamadı.");
            }

            return Ok(seferler); // Sefer listesini döndür
        }

        // GET: api/seferler/5
        // Belirli bir seferin detaylarını (ve koltuk durumunu) getirme
        [HttpGet("{id}")]
        public async Task<ActionResult<Sefer>> GetSeferDetay(int id)
        {
            var sefer = await _context.Seferler
                                    .Include(s => s.Guzergah)
                                    .Include(s => s.Otobus)
                                    .Include(s => s.Koltuklar) // Koltuk bilgilerini dahil et
                                    .FirstOrDefaultAsync(s => s.SeferID == id);

            if (sefer == null)
            {
                return NotFound("Sefer bulunamadı.");
            }

            // Burada seferin koltuklarının durumunu da (dolu/boş) döndürebiliriz.
            // İstenirse koltukları ayrı bir DTO (Data Transfer Object) ile döndürmek daha iyi olabilir.

            return Ok(sefer);
        }

        // --- Bilet Alma İşlemi İçin Farklı Bir Controller (örn: BiletController) Oluşturmak Daha Mantıklı ---
        // POST: api/biletler
        // [HttpPost("/api/biletler")] // Başka bir controller'da olmalı
        // public async Task<ActionResult<Bilet>> BiletAl([FromBody] BiletAlRequest request) { ... }
        // Bilet alma işlemi:
        // 1. Yolcu bilgilerini al/oluştur
        // 2. İstenen seferi bul
        // 3. İstenen koltuğun boş olup olmadığını kontrol et (Koltuklar tablosundan)
        // 4. Koltuğu 'dolu' olarak işaretle
        // 5. Bilet kaydını oluştur (PNR üret)
        // 6. Veritabanına kaydet
        // 7. Oluşturulan bileti döndür
    }
}