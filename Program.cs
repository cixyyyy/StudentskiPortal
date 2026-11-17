using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PrijavaIspita.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Registracija MVC servisa
builder.Services.AddControllersWithViews();

// 2. Registracija DbContext-a
builder.Services.AddDbContext<PrijavaIspitaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Registracija memorijskog cache-a i sesija
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// 4. Registracija HttpContextAccessor (ovo rešava tvoju grešku)
builder.Services.AddHttpContextAccessor();

// 5. Dodavanje autentikacije (cookie-based)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// 6. Dodavanje autorizacije (role-based)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("Student", policy => policy.RequireRole("Student"));
    options.AddPolicy("Profesor", policy => policy.RequireRole("Profesor"));
});

var app = builder.Build();

// 7. Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication(); // mora pre UseAuthorization
app.UseAuthorization();

// 8. Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
