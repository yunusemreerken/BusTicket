using BusTicket.Areas.Admin.Models.ViewModels;
using BusTicket.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BusTicket.Areas.Admin.Controllers
{
    [Area("Admin")] // <<<--- BU ÇOK ÖNEMLİ!

    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); // Başarılı giriş
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
            }

            
            
            
            
            
            // var user = await _userManager.FindByEmailAsync(model.Email);
            //
            // if (user != null)
            // {
            //     var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            //     
            //     
            //     if (result.Succeeded)
            //     {
            //         return RedirectToAction("Index", "Home");
            //     }
            // }
            //
            // ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        
        // GET: /Account/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User); // Mevcut giriş yapmış kullanıcıyı al
            if (user == null)
            {
                return NotFound($"Kullanıcı bulunamadı (ID: '{_userManager.GetUserId(User)}').");
            }

            var model = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                // Eğer özel alanlarınız varsa:
                // FirstName = user.FirstName,
                // LastName = user.LastName
            };

            // TempData ile başarı mesajı alınırsa modele ekle (POST sonrası redirect için)
            if (TempData["StatusMessage"] != null)
            {
                model.StatusMessage = TempData["StatusMessage"].ToString();
            }


            return View(model);
        }

        // POST: /Account/Profile
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF koruması
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Hata varsa formu tekrar göster
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Kullanıcı bulunamadı (ID: '{_userManager.GetUserId(User)}').");
            }

            // Telefon numarasını güncelle (diğer alanları benzer şekilde güncelleyin)
            var currentPhoneNumber = user.PhoneNumber;
            if (model.PhoneNumber != currentPhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Telefon numarası güncellenirken bir hata oluştu.");
                    return View(model);
                }
            }

            // Özel alanları güncelle (FirstName, LastName vb.)
            // bool profileUpdated = false;
            // if (user.FirstName != model.FirstName) { user.FirstName = model.FirstName; profileUpdated = true; }
            // if (user.LastName != model.LastName) { user.LastName = model.LastName; profileUpdated = true; }
            // if(profileUpdated) { await _userManager.UpdateAsync(user); }


            await _signInManager.RefreshSignInAsync(user); // Kullanıcı bilgilerini cookie'de güncelle

            // Başarı mesajını TempData ile bir sonraki request'e taşı
            TempData["StatusMessage"] = "Profiliniz başarıyla güncellendi."; 

            return RedirectToAction(nameof(Profile)); // Başarılı güncelleme sonrası Profile GET action'ına yönlendir
        }
        
        // GET: /Account/Settings
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Kullanıcı bulunamadı (ID: '{_userManager.GetUserId(User)}').");
            }

            var model = new SettingsViewModel();
            // Mevcut ayarları yükle (varsa)
            // model.EnableNotifications = user.NotificationsEnabled;

            if (TempData["SettingsStatusMessage"] != null)
            {
                model.StatusMessage = TempData["SettingsStatusMessage"].ToString();
            }

            return View(model);
        }

        // POST: /Account/Settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(SettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Kullanıcı bulunamadı (ID: '{_userManager.GetUserId(User)}').");
            }

            // Ayarları güncelle
            // bool settingsChanged = false;
            // if(user.NotificationsEnabled != model.EnableNotifications) { user.NotificationsEnabled = model.EnableNotifications; settingsChanged = true; }
            // if(settingsChanged) { await _userManager.UpdateAsync(user); }

            await _signInManager.RefreshSignInAsync(user);
            TempData["SettingsStatusMessage"] = "Ayarlarınız başarıyla güncellendi.";
            return RedirectToAction(nameof(Settings));
        }
        
    }
}