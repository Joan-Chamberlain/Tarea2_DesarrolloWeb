using DiarioWeb.Models;
using DiarioWeb.Models.ViewModels;
using DiarioWeb.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

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
    public async Task<IActionResult> ViewPost(int Id)
    {
      var post = _context.Posts.Where(b=>b.Id == Id).FirstOrDefault();

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
          Texto = model.Texto?.Replace("\r\n", "<br>"),
          Imagen = imagenBytes,
          Categoría = model.Categoria,
          AuthorId = userID
        };

        _context.Add(post);
        await _context.SaveChangesAsync();

        var Id = _context.Posts.Where(b => b.Fecha == post.Fecha && b.AuthorId == userID).FirstOrDefault().Id;

        return RedirectToAction("ViewPost", new { Id = Id });
      }

      CategoryManager categoryManager = new CategoryManager();
      List<string> categorias = categoryManager.ObtenerCategorias();

      ViewData["Categorías"] = categorias;

      return View();

    }

            [HttpGet]
        public async Task<IActionResult> PostsByAuthor(string author)
        {
            var user =  _context.Users.Where(b => b.Id == author).FirstOrDefault();

            //var user = await _userManager.FindByNameAsync(author);

            if (user == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts
                .Include(p => p.Author) // Include the Author navigation property
                //.Include(p => p.Comments) // Include the Comments navigation property
                .Where(p => p.AuthorId == user.Id)
                .ToListAsync();

            return View(posts);
        }


        [HttpGet]
        public async Task<IActionResult> PostsByCategorie(string categorie)
        {
 
            var posts = await _context.Posts
                 .Include(p => p.Author)
                .Where(p => p.Categoría == categorie)
                .ToListAsync();

            return View(posts);
        }
  }
}
