using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Yetkilendirme için
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicket.Data;
using BusTicket.Models.Entities;

namespace BusTicket.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Sadece Admin erişebilir
    public class GuzergahlarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GuzergahlarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Guzergahlar (Listeleme)
        public async Task<IActionResult> Index()
        {
            // Tüm güzergahları listele, Kalkış şehrine göre sırala
            return View(await _context.Guzergahlar.OrderBy(g => g.KalkisSehri).ThenBy(g => g.VarisSehri).ToListAsync());
        }

        // GET: Admin/Guzergahlar/Details/5 (Detay)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guzergah = await _context.Guzergahlar
                .FirstOrDefaultAsync(m => m.GuzergahID == id);
            if (guzergah == null)
            {
                return NotFound();
            }

            return View(guzergah);
        }

        // GET: Admin/Guzergahlar/Create (Oluşturma Formu)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Guzergahlar/Create (Oluşturma İşlemi)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KalkisSehri,VarisSehri")] Guzergah guzergah)
        {
            // Aynı kalkış ve varış şehri girilemez gibi ek kontroller yapılabilir
             if (guzergah.KalkisSehri == guzergah.VarisSehri)
             {
                  ModelState.AddModelError("", "Kalkış ve Varış şehri aynı olamaz.");
             }

            // Bu güzergah zaten var mı diye kontrol edilebilir
            bool zatenVar = await _context.Guzergahlar.AnyAsync(g => g.KalkisSehri == guzergah.KalkisSehri && g.VarisSehri == guzergah.VarisSehri);
            if(zatenVar)
            {
                 ModelState.AddModelError("", "Bu güzergah zaten kayıtlı.");
            }


            if (ModelState.IsValid)
            {
                _context.Add(guzergah);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(guzergah);
        }

        // GET: Admin/Guzergahlar/Edit/5 (Düzenleme Formu)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guzergah = await _context.Guzergahlar.FindAsync(id);
            if (guzergah == null)
            {
                return NotFound();
            }
            return View(guzergah);
        }

        // POST: Admin/Guzergahlar/Edit/5 (Düzenleme İşlemi)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GuzergahID,KalkisSehri,VarisSehri")] Guzergah guzergah)
        {
            if (id != guzergah.GuzergahID)
            {
                return NotFound();
            }

             if (guzergah.KalkisSehri == guzergah.VarisSehri)
             {
                  ModelState.AddModelError("", "Kalkış ve Varış şehri aynı olamaz.");
             }

            // Düzenlenen güzergah başka bir ID ile zaten var mı? (Kendisi hariç)
             bool zatenVar = await _context.Guzergahlar.AnyAsync(g => g.KalkisSehri == guzergah.KalkisSehri && g.VarisSehri == guzergah.VarisSehri && g.GuzergahID != id);
             if(zatenVar)
             {
                 ModelState.AddModelError("", "Bu güzergah zaten kayıtlı.");
             }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guzergah);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuzergahExists(guzergah.GuzergahID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(guzergah);
        }

        // GET: Admin/Guzergahlar/Delete/5 (Silme Onay Sayfası)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guzergah = await _context.Guzergahlar
                .FirstOrDefaultAsync(m => m.GuzergahID == id);
            if (guzergah == null)
            {
                return NotFound();
            }

             // Bu güzergahı kullanan sefer var mı kontrol et
            bool seferVar = await _context.Seferler.AnyAsync(s => s.GuzergahID == id);
            ViewData["SeferVar"] = seferVar;
            if (seferVar)
            {
                ViewData["UyariMesaji"] = "Bu güzergahı kullanan seferler bulunmaktadır. Silme işlemi yapılamaz!";
            }

            return View(guzergah);
        }

        // POST: Admin/Guzergahlar/Delete/5 (Silme İşlemi)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             // Tekrar sefer kontrolü yap
             bool seferVar = await _context.Seferler.AnyAsync(s => s.GuzergahID == id);
             if (seferVar)
             {
                  var guzergahForError = await _context.Guzergahlar.FindAsync(id); // Hata sayfası için modeli al
                  ViewData["SeferVar"] = seferVar;
                  ViewData["UyariMesaji"] = "Bu güzergahı kullanan seferler bulunmaktadır. Silme işlemi yapılamaz!";
                  return View("Delete", guzergahForError); // Silme sayfasına model ve uyarı ile geri dön
             }


            var guzergah = await _context.Guzergahlar.FindAsync(id);
            if (guzergah != null)
            {
                _context.Guzergahlar.Remove(guzergah);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuzergahExists(int id)
        {
            return _context.Guzergahlar.Any(e => e.GuzergahID == id);
        }
    }
}