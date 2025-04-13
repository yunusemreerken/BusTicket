using Microsoft.AspNetCore.Identity; // Ekleyin
using Microsoft.EntityFrameworkCore;
using BusTicket.Data;
using BusTicket.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ----- IDENTITY SERVİSLERİNİ EKLEME -----
// Varsayılan IdentityUser ve IdentityRole kullanarak Identity ekle
// AddRoles<IdentityRole>() rol yönetimini etkinleştirir
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        // ----- GEÇİCİ TEST İÇİN GEVŞET -----
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 1; // Minimum uzunluk
        options.Password.RequiredUniqueChars = 0;
        // ----- GEÇİCİ TEST BİTTİ -----
        
        // Şifre ayarları (isteğe bağlı, varsayılanlar da kullanılabilir)
        // options.Password.RequireDigit = false;
        // options.Password.RequireLowercase = false;
        // options.Password.RequireNonAlphanumeric = false;
        // options.Password.RequireUppercase = false;
        // options.Password.RequiredLength = 6; // Örnek, daha güvenli bir uzunluk seçin
        // options.Password.RequiredUniqueChars = 1;

        // Oturum açma ayarları
        options.SignIn.RequireConfirmedAccount = false; // E-posta doğrulaması istiyorsanız true yapın
        options.SignIn.RequireConfirmedEmail = false;   // E-posta doğrulaması istiyorsanız true yapın
        // options.SignIn.RequireConfirmedPhoneNumber = false;

        // Kullanıcı ayarları
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true; // Email'lerin benzersiz olmasını sağla
    })
    .AddRoles<IdentityRole>() // Rol yönetimini ekle
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
// Veritabanı bağlantısını yap

// ----- IDENTITY SERVİSLERİ EKLENDİ -----


builder.Services.AddControllersWithViews(); // Veya AddControllers() + AddRazorPages()

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ----- AUTHENTICATION VE AUTHORIZATION MIDDLEWARE'LERİNİ EKLEME -----
// ÖNEMLİ: UseAuthentication(), UseAuthorization()'dan ÖNCE gelmelidir!
app.UseAuthentication(); // Kimlik doğrulama middleware'ini ekle
app.UseAuthorization(); // Yetkilendirme middleware'ini ekle
// ----- MIDDLEWARE'LER EKLENDİ -----


app.UseEndpoints(endpoints =>
{
    // app.UseEndpoints içinde
    endpoints.MapControllerRoute(
        name: "AdminArea",
        pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}" // defaults kısmı kaldırıldı
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Identity UI sayfaları için Razor Pages'i map et (Login, Register vb.)
    endpoints.MapRazorPages(); // <-- Identity UI için bunu ekleyin
});




// ----- VERİTABANI SEED İŞLEMİ (ADMIN KULLANICISI OLUŞTURMA) -----
// Uygulama ilk çalıştığında Admin rolünü ve kullanıcısını oluşturmak için
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // SeedData sınıfını ve metodunu birazdan oluşturacağız
        await SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı seed işlemi sırasında bir hata oluştu.");
    }
}
// ----- SEED İŞLEMİ BİTTİ -----


app.Run();