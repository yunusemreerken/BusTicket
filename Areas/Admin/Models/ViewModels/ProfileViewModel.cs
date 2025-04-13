using System.ComponentModel.DataAnnotations;

namespace BusTicket.Areas.Admin.Models.ViewModels;

public class ProfileViewModel
{
    // Görüntülenecek ama değiştirilemeyecek alanlar
    [Display(Name = "Kullanıcı Adı")]
    public string? UserName { get; set; }

    [Display(Name = "E-posta")]
    public string? Email { get; set; }

    // Formda güncellenebilecek alanlar
    [Display(Name = "Adınız")]
    [StringLength(50)]
    public string? FirstName { get; set; } // IdentityUser'da yok, özel alansa ekleyin

    [Display(Name = "Soyadınız")]
    [StringLength(50)]
    public string? LastName { get; set; } // IdentityUser'da yok, özel alansa ekleyin

    [Phone]
    [Display(Name = "Telefon Numarası")]
    public string? PhoneNumber { get; set; } // IdentityUser'da var

    // Başarı/Hata mesajı göstermek için
    public string? StatusMessage { get; set; }
}