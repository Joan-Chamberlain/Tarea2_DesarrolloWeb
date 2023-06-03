using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DiarioWeb.Models
{
  public partial class PostDbContext : IdentityDbContext<ApplicationUser>
  {
    public PostDbContext()
    {

    }

    public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
    {
    }

    // Agrega tus modelos personalizados aquí
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    // Configuraciones adicionales, relaciones, etc.
  }
}
