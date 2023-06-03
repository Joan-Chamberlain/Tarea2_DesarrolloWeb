using Microsoft.EntityFrameworkCore;


namespace DiarioWeb.Models
{
  public class PostDbContext : DbContext
  {
    public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
    {
    }

    // Agrega tus modelos personalizados aquí
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    // Configuraciones adicionales, relaciones, etc.
  }
}
