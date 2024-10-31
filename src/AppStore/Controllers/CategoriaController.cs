using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppStore.Repositories.Abstract;
using AppStore.Repositories.Implementation;
using AppStore.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace AppStore.Controllers
{
    [Authorize]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpPost]
        public IActionResult Add(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }
            if (string.IsNullOrEmpty(categoria.Nombre))
            {
                TempData["msg"] = "Por favor ingrese el nombre de la categoría";
                return View(categoria);
            }

            var categoriaAgregada = _categoriaService.Add(categoria);

            if (categoriaAgregada)
            {
                TempData["msg"] = "Categoría agregada correctamente";
                return RedirectToAction(nameof(Add));
            }
            TempData["msg"] = "Error al agregar la categoría";
            return View(categoria);

        }

        [HttpPost]
        public IActionResult Edit(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }
            if (string.IsNullOrEmpty(categoria.Nombre))
            {
                TempData["msg"] = "Ingrese un nombre de categoría válido";
                return View(categoria);
            }


            var resultadoCategoria = _categoriaService.Update(categoria);

            if (!resultadoCategoria)
            {
                TempData["msg"] = "Error al actualizar la categoria";
                return View(categoria);
            }

            TempData["msg"] = "Categoría actualizada correctamente";
            return View(categoria);

        }

        public IActionResult Add()
        {
            var categoria = new Categoria();
            return View(categoria);
        }
        public IActionResult Edit(int id)
        {
            var categoria = _categoriaService.GetById(id);
            if (categoria == null)
            {
                TempData["msg"] = "Error, categoría inexistente";
                return RedirectToAction(nameof(CategoriasList));
            }

            return View(categoria);
        }
        public IActionResult CategoriasList()
        {
            var categorias = _categoriaService.List();
            return View(categorias);
        }
        public IActionResult Delete(int id)
        {
            _categoriaService.Delete(id);
            return RedirectToAction(nameof(CategoriasList));
        }
    }
}