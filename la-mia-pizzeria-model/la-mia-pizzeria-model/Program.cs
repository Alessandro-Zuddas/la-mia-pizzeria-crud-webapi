using la_mia_pizzeria_model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using la_mia_pizzeria_model.Areas.Identity.Data;
using PizzaContextIdentity = la_mia_pizzeria_model.Areas.Identity.Data.PizzaContext;
using PizzaContext = la_mia_pizzeria_model.Models.PizzaContext;

var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddDbContext<PizzaContextIdentity>(options =>
    options.UseSqlServer("Data Source=localhost;Initial Catalog=PizzaDb;Integrated Security=True;Pooling=False;TrustServerCertificate = True"));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PizzaContextIdentity>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pizza}/{action=Index}/{id?}");

app.MapRazorPages();

using (var ctx = new PizzaContext())
{
    ctx!.Seed();
}

app.Run();
