using Cinemagic.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// הוספת שירותי Session ושירותי HttpContextAccessor
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// חיבור לבסיס הנתונים SQL Server
builder.Services.AddDbContext<CinemagicContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// הפעלת סשן - חובה לפני UseAuthorization אם רוצים להשתמש בסשן בתוך Authentication/Authorization
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

