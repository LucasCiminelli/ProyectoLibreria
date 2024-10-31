using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStore.Models.Domain;
using AppStore.Models.DTO;
using AppStore.Repositories.Abstract;

namespace AppStore.Repositories.Implementation
{
    public class LibroService : ILibroService
    {

        private readonly DatabaseContext ctx;

        public LibroService(DatabaseContext ctxParametro)
        {
            this.ctx = ctxParametro;
        }

        public bool Add(Libro libro)
        {
            try
            {
                ctx.Libros.Add(libro);
                ctx.SaveChanges();
                foreach (int categoriaId in libro.Categorias!)
                {
                    var libroCategoria = new LibroCategoria
                    {
                        LibroId = libro.Id,
                        CategoriaId = categoriaId
                    };
                    ctx.LibroCategorias.Add(libroCategoria);
                }

                ctx.SaveChanges();

                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

        public bool Detele(int id)
        {
            try
            {
                var data = this.GetById(id);

                if (data is null)
                {
                    return false;
                }

                var libroCategorias = ctx.LibroCategorias!.Where(a => a.LibroId == data.Id); //eliminando referencias del libro dentro de libroCategorias

                ctx.LibroCategorias.RemoveRange(libroCategorias);
                ctx.Libros!.Remove(data);

                ctx.SaveChanges();

                return true;


            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public Libro GetById(int id)
        {
            try
            {
                return ctx.Libros!.Find(id)!;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<int> GetCategoriaByLibroId(int libroId)
        {
            return ctx.LibroCategorias.Where(x => x.LibroId == libroId).Select(x => x.CategoriaId).ToList();
        }

        public LibroListVm List(string term = "", bool paging = false, int currentPage = 0)
        {
            var data = new LibroListVm();

            var list = ctx.Libros!.ToList();

            if (!String.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                list = list.Where(x => x.Titulo!.ToLower().Contains(term)).ToList();
            }
            if (paging)
            {
                int pageSize = 3;
                int count = list.Count();
                int totalPages = (int)Math.Ceiling(count / (double)pageSize);

                list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                data.PageSize = pageSize;
                data.CurrentPage = currentPage;
                data.TotalPages = totalPages;
                data.Term = term;

            }

            foreach (var libro in list)
            {
                var categorias = (
                    from categoria in ctx.Categorias
                    join lc in ctx.LibroCategorias
                    on categoria.Id equals lc.CategoriaId
                    where lc.LibroId == libro.Id
                    select categoria.Nombre
                ).ToList(); //devuelve una cadena de categorias strings.


                string categoriaNombres = string.Join(",", categorias); //joinea las categorias separados por una coma. "drama, horror, comedia"...
                libro.CategoriasNames = categoriaNombres;

            }

            data.LibroList = list.AsQueryable();


            return data;
        }

        public bool Update(Libro libro)
        {
            try
            {
                var categoriasParaEliminar = ctx.LibroCategorias!.Where(x => x.LibroId == libro.Id);

                foreach (var categoria in categoriasParaEliminar)
                {
                    ctx.LibroCategorias.Remove(categoria);
                }

                foreach (int categoriaId in libro.Categorias!)
                {
                    var libroCategoria = new LibroCategoria
                    {
                        CategoriaId = categoriaId,
                        LibroId = libro.Id
                    };

                    ctx.LibroCategorias.Add(libroCategoria);
                }

                ctx.Libros.Update(libro);
                ctx.SaveChanges();

                return true;

            }
            catch (Exception)
            {
                return false;


            }
        }
    }
}