using DiarioWeb.Models;
using DiarioWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DiarioWeb.Controllers
{

  [Authorize]
  public class HomeController : Controller
  {
    private readonly PostDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(PostDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    public IActionResult Index()
    {
      return View();
    }

    public async Task<IActionResult> AdminView()
    {
      var users = await _context.Users.ToListAsync();

      if (users != null)
      {
        var roleList = await _context.Roles.Select(r => r.Name).ToListAsync();

        UserListViewModel userList = new UserListViewModel();
        userList.Name = new string[users.Count];
        userList.Email = new string[users.Count];
        userList.Role = new string[users.Count];

        int iterator = 0;

        foreach (var user in users)
        {
          userList.Name[iterator] = user.FirstName + " " + user.LastName;
          userList.Email[iterator] = user.Email;

          var userRole = _context.UserRoles.Where(b => b.UserId == user.Id).FirstOrDefault();
          if (userRole != null)
          {
            userList.Role[iterator] = _context.Roles.FirstOrDefault(a => a.Id == userRole.RoleId)?.Name;
          }
          iterator++;
        }

        ViewData["Users"] = userList;
        ViewData["Roles"] = roleList;
      }

      return View();
    }

    [HttpPost]
    public async Task<IActionResult> EliminarUsuario(string Email)
    {
      
      var usuario = _context.Users.Where(b=>b.Email == Email).FirstOrDefault();

      if (usuario != null)
      {
        _context.Users.Remove(usuario);
        await _context.SaveChangesAsync();
      }

      return RedirectToAction("AdminView");
    }

    [HttpGet]
    public async Task<IActionResult> ActualizarUsuario(string Email)
    {
      var user = await _context.Users.Where(b=>b.Email == Email).FirstOrDefaultAsync();

      var roles = await _context.Roles.Select(b => b.Name).ToListAsync();
      ViewData["Roles"] = roles;

      EditUserViewModel model = new EditUserViewModel();
      model.FirstName = user.FirstName;
      model.LastName = user.LastName;
      model.Email = user.Email;
      model.Role = _context.Roles.Where(a => a.Id == _context.UserRoles.Where(b=>b.UserId == user.Id).FirstOrDefault().RoleId).FirstOrDefault().Name;


      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ActualizarUsuario(EditUserViewModel user)
    {
      if (ModelState.IsValid) 
      {
        var oldUser = _context.Users.Where(b => b.Email == user.Email).FirstOrDefault();
        var oldRole = _context.Roles.Where(a => a.Id == _context.UserRoles.Where(b => b.UserId == oldUser.Id).FirstOrDefault().RoleId).FirstOrDefault().Name;
        if (oldUser != null)
        {
          await _userManager.RemoveFromRoleAsync(oldUser, oldRole);
          await _userManager.AddToRoleAsync(oldUser, user.Role);

          oldUser.FirstName = user.FirstName;
          oldUser.LastName = user.LastName;

          await _context.SaveChangesAsync();

          return RedirectToAction("AdminView");
        }
      }

      var roles = await _context.Roles.Select(b => b.Name).ToListAsync();
      ViewData["Roles"] = roles;

      return View(user);
    }

  }
}