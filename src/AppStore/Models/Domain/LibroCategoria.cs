namespace AppStore.Models.Domain;

public class LibroCategoria
{
    public int Id { get; set; }
    public int CategoriaId { get; set; }
    public int LibroId { get; set; }

    public Libro? Libro { get; set; } //anclas, referencias a la clase libro
    public Categoria? Categoria { get; set; } //anclas, referencias a la clase Categoria
}