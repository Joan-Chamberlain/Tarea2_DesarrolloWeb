namespace DiarioWeb.Models
{
  public class Post
  {
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Titulo { get; set; }
    public string Imagen { get; set; }
    public string Encabezado { get; set; }
    public string AuthorId { get; set; } // Foreign key

    public ApplicationUser Author { get; set; } // Relación con el autor
    public ICollection<Comment> Comments { get; set; } // Relación con los comentarios
  }
}
