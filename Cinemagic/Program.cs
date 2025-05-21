using Cinemagic.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// ����� ������ Session ������� HttpContextAccessor
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// ����� ����� ������� SQL Server
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

// ����� ��� - ���� ���� UseAuthorization �� ����� ������ ���� ���� Authentication/Authorization
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

