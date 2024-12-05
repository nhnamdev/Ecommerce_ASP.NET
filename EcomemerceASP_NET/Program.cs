﻿using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EcommerceContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("Ecommerce"));
    });
//Add service database
// Add services to the container.

// Add session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add session

// Auto reload khi thay đổi file cshtml
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

// Auto reload khi thay đổi file cshtml

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
// Add AutoMapper

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

app.UseRouting();

app.UseSession();

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=Index}/{id?}");
app.Run();
