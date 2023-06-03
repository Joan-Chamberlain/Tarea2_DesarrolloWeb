namespace DiarioWeb.Models
{
  public class Comment
  {
    public int Id { get; set; }
    public string Contenido { get; set; }
    public DateTime Fecha { get; set; }
    public int PostId { get; set; } // Foreign key

    public Post Post { get; set; } // Relación con la publicación
  }
}
