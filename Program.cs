﻿using Hotel.Data;
using Hotel.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Thêm dịch vụ PayPalClient
builder.Services.AddSingleton(x =>
    new PaypalClient(
        builder.Configuration["PayPalOptions:ClientId"],
        builder.Configuration["PayPalOptions:ClientSecret"],
        builder.Configuration["PayPalOptions:Mode"]
    )
);
builder.Services.AddDbContext<HotelDbContext>(o =>
{
	o.UseSqlServer(builder.Configuration.GetConnectionString("DemoConnectionString"));
});

builder.Services.AddScoped<BaseDataActionFilter>();

// add authozied use cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
	options.LoginPath = "/login";
	options.AccessDeniedPath = "/unauthozied";
	options.ExpireTimeSpan = TimeSpan.FromDays(1);
});



builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});

// Add session services
builder.Services.AddSession();
builder.Services.AddSignalR();
builder.Services.AddMvc(options =>
{
	options.Filters.Add(typeof(BaseDataActionFilter));
	options.Filters.Add(new AuthorizeFilter());
});


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

app.UseAuthentication();
app.UseAuthorization();
// Use session middleware
app.UseSession();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/chathub");
app.Run();
