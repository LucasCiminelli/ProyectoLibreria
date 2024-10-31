using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace AppStore.Models.Domain;


public class Libro
{
    [Key] //notaciones
    [Required]//notaciones
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? CreateDate { get; set; }
    public string? Imagen { get; set; }
    [Required]
    public string? Autor { get; set; }

    public virtual ICollection<Categoria>? CategoriaRelationList { get; set; }

    public virtual ICollection<LibroCategoria>? LibroCategoriaRelationList { get; set; }

    [NotMapped] //no lo va a mapear en la base de datos, solo va a funcionar a nivel objetos.
    public List<int>? Categorias { get; set; }

    [NotMapped]
    public string? CategoriasNames { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    [NotMapped]
    public IEnumerable<SelectListItem>? CategoriasList { get; set; }
    [NotMapped]
    public MultiSelectList? MultiCategoriasList { get; set; }

}