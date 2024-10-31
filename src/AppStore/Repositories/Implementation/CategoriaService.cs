using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStore.Models.Domain;
using AppStore.Repositories.Abstract;

namespace AppStore.Repositories.Implementation
{
    public class CategoriaService : ICategoriaService
    {

        private readonly DatabaseContext _ctx;

        public CategoriaService(DatabaseContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<Categoria> List()
        {
            return _ctx.Categorias!.AsQueryable();
        }

        public bool Add(Categoria categoria)
        {
            try
            {
                _ctx.Categorias.Add(categoria);
                _ctx.SaveChanges();

                return true;
            }
            catch (Exception)
            {

                return false;
                throw;
            }
        }

        public bool Update(Categoria categoria)
        {

            try
            {
                var categoriaSeleccionada = _ctx.Categorias.FirstOrDefault(x => x.Id == categoria.Id);
                if (categoriaSeleccionada == null)
                {
                    return false;
                }
                categoriaSeleccionada.Nombre = categoria.Nombre;

                _ctx.Categorias.Update(categoriaSeleccionada);
                _ctx.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }


        }

        public bool Delete(int id)
        {
            try
            {
                var categoriaAEliminar = this.GetById(id);
                if (categoriaAEliminar == null)
                {
                    return false;
                }
                _ctx.Categorias.Remove(categoriaAEliminar);
                _ctx.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public Categoria GetById(int id)
        {
            try
            {
                return _ctx.Categorias!.Find(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
    }
}