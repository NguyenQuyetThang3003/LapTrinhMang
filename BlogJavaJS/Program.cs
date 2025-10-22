using BlogJavaJS.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===================== MVC =====================
builder.Services.AddControllersWithViews();

// ===================== PostgreSQL CONFIG =====================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    "Host=dpg-d3sbd36r433s73cnb4t0-a.singapore-postgres.render.com;" +
    "Port=5432;" +
    "Database=blogjavdb;" +
    "Username=renderuser;" +  // ✅ Dùng Username, KHÔNG phải User
    "Password=ZjUQdovklu24LjWQq6Qyoy1RE0DrVSn3;" +
    "SSL Mode=Require;" +
    "Trust Server Certificate=true;";

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Middleware & route
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
    db.Database.Migrate();
}

app.Run();
