using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStore.Models.Domain;

namespace AppStore.Repositories.Abstract
{
    public interface ICategoriaService
    {
        IQueryable<Categoria> List();

        bool Add(Categoria categoria);

        bool Update(Categoria categoria);

        bool Delete(int id);
        Categoria GetById(int id);
    }
}