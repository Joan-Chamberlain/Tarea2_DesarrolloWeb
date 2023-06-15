using System.ComponentModel.DataAnnotations;

namespace DiarioWeb.Models.ViewModels
{
  public class EditUserViewModel
  {
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Nombre")]
    public string FirstName { get; set; }

    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Apellidos")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; }

    [Required]
    public string Role { get; set; }
  }
}
