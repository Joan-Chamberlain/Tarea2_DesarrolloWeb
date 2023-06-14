using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DiarioWeb.Models.ViewModels
{
  public class PostViewModel
  {
    [Required]
    [DisplayName("Título del post")]
    public string Titulo { get; set; }

    [Required]
    public IFormFile Imagen { get; set; }

    [Required]
    public string Categoria { get; set; }

    [Required]
    public string Texto { get; set; }
  }
}
