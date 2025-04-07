using Microsoft.AspNetCore.Identity;
using BusTicket.Data;

namespace BusTicket.Models // Veya projenize uygun başka bir namespace
{
    public static class SeedData
    {
        // Metodun async olması ve IServiceProvider alması önemli
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            // Kapsam (scope) oluşturarak servisleri almalıyız
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); // Gerekirse DbContext'i de alabilirsiniz

                
                // ----- ROLLERİ OLUŞTUR -----
                string[] roleNames = { "Admin", "Uye" }; // Örnek roller
                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        // Rol yoksa oluştur
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // ----- ÖRNEK BİR ADMİN KULLANICISI OLUŞTUR -----
                var adminUserEmail = "admin@admin.com"; // Kendi admin emailinizi kullanın
                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);

                if (adminUser == null)
                {
                    var newAdminUser = new IdentityUser()
                    {
                        UserName = "admin", // Kullanıcı adı
                        Email = adminUserEmail,
                        EmailConfirmed = true // E-posta doğrulaması gerektirmiyorsanız true yapın
                        // Diğer IdentityUser özelliklerini de ayarlayabilirsiniz (PhoneNumber vb.)
                    };
                    
                    // Şifreyi burada belirleyin (Güvenli bir şifre seçin!)
                    var result = await userManager.CreateAsync(newAdminUser, "Sifre123");
                    
                    if (result.Succeeded)
                    {
                        // Kullanıcı başarıyla oluşturulduysa Admin rolüne ata
                        await userManager.AddToRoleAsync(newAdminUser, "Admin");
                    }
                    // else kısmında result.Errors loglanabilir
                }
                // Başka seed işlemleri (varsa) buraya eklenebilir
            }
        }
    }
}