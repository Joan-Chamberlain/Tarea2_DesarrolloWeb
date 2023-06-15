using DiarioWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiarioWeb.Controllers
{
  public class CommentController : Controller
  {

    private readonly PostDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CommentController(PostDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> CommentView(int Id)
    {
      var comments = await _context.Comments.Where(b=>b.PostId == Id).ToListAsync();
      ViewData["comments"] = comments;
      ViewBag.ID = Id;

      return View();
    }

    [HttpPost]
    public async Task<IActionResult> CommentView(int Id, string commentText)
    {
      if (ModelState.IsValid)
      {
        Comment comment = new Comment()
        {
          PostId = Id,
          Contenido = commentText,
          Fecha = DateTime.Now
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

      }

      var comments = await _context.Comments.Where(b => b.PostId == Id).ToListAsync();
      ViewData["comments"] = comments;
      ViewBag.ID = Id;

      return View();
    }
  }
}
