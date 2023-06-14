namespace DiarioWeb.Utility
{
  public class CategoryManager
  {
    public List<string> ObtenerCategorias()
    {
      List<string> categorias = new List<string>();

      categorias = File.ReadAllLines("./Utility/Categorías.txt").ToList();

      return categorias;
    }
  }
}
