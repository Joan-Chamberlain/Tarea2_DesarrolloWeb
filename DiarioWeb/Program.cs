using DiarioWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<PostDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
  options.SignIn.RequireConfirmedAccount = false;
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequiredLength = 1;
  options.Password.RequireLowercase= false;
  options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<PostDbContext>();

builder.Services.AddAuthentication()
  .AddGoogle(googleOptions =>
    {
      googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
      googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}" ,
    app.MapRazorPages());

app.Run();
