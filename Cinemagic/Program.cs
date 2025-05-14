using Cinemagic.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// ? ����� ������ Session
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor(); // ��� ����� ������ ���� �������� ������

// ����� ����� �������
builder.Services.AddDbContext<CinemagicContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// ? ����� ���� �����
app.UseSession();

app.MapRazorPages();

app.Run();
