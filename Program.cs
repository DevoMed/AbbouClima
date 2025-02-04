using AbbouClima.Data;
using AbbouClima.Services;
using AbbouClima.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using jsreport.Client;
using jsreport.Binary;
using jsreport.Local;
using jsreport.AspNetCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPresupuestoService, PresupuestoService>();
builder.Services.Configure<GlobalSettings>(builder.Configuration.GetSection("GlobalSettings"));
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//builder.Services.AddJsReport(new ReportingService("http://localhost:5488"));
builder.Services.AddJsReport(new LocalReporting()
        .UseBinary(jsreport.Binary.JsReportBinary.GetBinary())
        .KillRunningJsReportProcesses()
        .AsUtility()
        .Create());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/Home/Error");

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
