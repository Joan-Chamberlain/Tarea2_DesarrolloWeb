using DiarioWeb.Models;
using DiarioWeb.Models.ViewModels;
using DiarioWeb.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> ViewPost() // int Id
    {
      var post = _context.Posts.Where(b=>b.Id == 1003).FirstOrDefault();

      var author = _context.Users.Where(b=>b.Id == post.AuthorId).FirstOrDefault();

      ViewBag.Post = post;
      ViewBag.Author = author;

      return View();
    }

    [HttpGet]
    public async Task<IActionResult> CreatePost()
    {
      CategoryManager categoryManager = new CategoryManager();
      List<string> categorias = categoryManager.ObtenerCategorias();

      ViewData["Categorías"] = categorias;

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
