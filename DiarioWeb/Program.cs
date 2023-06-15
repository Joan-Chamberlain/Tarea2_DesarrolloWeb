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
  .AddRoles<IdentityRole>()
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
    pattern: "{controller=Home}/{action=DisplayPosts}/{id?}",
    app.MapRazorPages());

using (var scope = app.Services.CreateScope())
{
  var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

  var roles = new[] { "Admin", "Member" };

  foreach (var role in roles)
  {
    if (!await roleManager.RoleExistsAsync(role))
    {
      await roleManager.CreateAsync(new IdentityRole(role));
    }
  }
}

using (var scope = app.Services.CreateScope())
{
  var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

  var email = "admin@admin.com";
  var password = "123123";

  if (await userManager.FindByEmailAsync(email) == null)
  {
    var user = new ApplicationUser();
    user.Email = email;
    user.UserName = email;
    user.FirstName = "Admin";
    user.LastName = "Admin";
    user.EmailConfirmed= true;

    await userManager.CreateAsync(user, password);

    await userManager.AddToRoleAsync(user, "Admin");
  }
}

app.Run();
