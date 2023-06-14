﻿using DiarioWeb.Models;
using DiarioWeb.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiarioWeb.Controllers
{
  public class PostsController : Controller
  {

    private readonly PostDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PostsController(PostDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> CreatePost()
    {
      
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePost(PostViewModel model)
    {
      var userID = _userManager.GetUserId(User);

      if (ModelState.IsValid)
      {
        byte[] imagenBytes;
        using (var stream = new MemoryStream())
        {
          await model.Imagen.CopyToAsync(stream);
          imagenBytes = stream.ToArray();
        }

        var post = new Post()
        {
          Fecha = DateTime.Now,
          Titulo = model.Titulo,
          Texto = model.Texto,
          Imagen = imagenBytes,
          Categoría = model.Categoria,
          AuthorId = userID
        };

        _context.Add(post);
        await _context.SaveChangesAsync();

      }
      return View();
    }
  }
}