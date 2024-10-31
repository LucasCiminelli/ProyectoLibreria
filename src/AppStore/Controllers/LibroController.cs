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
    public class LibroController : Controller
    {
        private readonly ILibroService _libroService;
        private readonly IFileService _fileService;
        private readonly ICategoriaService _categoriaService;

        public LibroController(ILibroService libroService, IFileService fileService, ICategoriaService categoriaService)
        {
            _libroService = libroService;
            _fileService = fileService;
            _categoriaService = categoriaService;
        }

        [HttpPost]
        public IActionResult Add(Libro libro)
        {
            libro.CategoriasList = _categoriaService.List().Select(a => new SelectListItem { Text = a.Nombre, Value = a.Id.ToString() });


            if (!ModelState.IsValid)
            {
                return View(libro);
            }

            if (libro.ImageFile != null)
            {
                var resultado = _fileService.SaveImage(libro.ImageFile);
                if (resultado.Item1 == 0)
                {
                    TempData["msg"] = resultado.Item2;
                    return View(libro);
                }

                var imagenName = resultado.Item2;
                libro.Imagen = imagenName;

            }

            var resultadoLibro = _libroService.Add(libro);

            if (resultadoLibro)
            {
                TempData["msg"] = "Se agregó correctamente";
                return RedirectToAction(nameof(Add));
            }

            TempData["msg"] = "Error al guardar el libro";
            return View(libro);



        }

        [HttpPost]
        public IActionResult Edit(Libro libro)
        {
            var categoriasDelLibro = _libroService.GetCategoriaByLibroId(libro.Id);
            var multiSelectListCategorias = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", categoriasDelLibro);
            libro.MultiCategoriasList = multiSelectListCategorias;


            if (!ModelState.IsValid)
            {
                return View(libro);
            }

            if (libro.ImageFile != null)
            {
                var fileNuevo = _fileService.SaveImage(libro.ImageFile);
                if (fileNuevo.Item1 == 0)
                {
                    TempData["msg"] = "La imagen no fue guardada";
                    return View(libro);
                }

                var imageName = fileNuevo.Item2;
                libro.Imagen = imageName;
            }
            var resultadoLibro = _libroService.Update(libro);

            if (!resultadoLibro)
            {
                TempData["msg"] = "Error en la actualización del libro";
                return View(libro);
            }

            TempData["msg"] = "Libro actualizado correctamente";
            return View(libro);

        }


        public IActionResult Add()
        {
            var libro = new Libro();
            libro.CategoriasList = _categoriaService.List().Select(a => new SelectListItem { Text = a.Nombre, Value = a.Id.ToString() });
            return View(libro);
        }
        public IActionResult Edit(int id)
        {
            var libro = _libroService.GetById(id);
            var categoriasDelLibro = _libroService.GetCategoriaByLibroId(id);

            var multiSelectListCategorias = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", categoriasDelLibro);

            libro.MultiCategoriasList = multiSelectListCategorias;

            return View(libro);
        }
        public IActionResult LibroList()
        {
            var libros = _libroService.List();
            return View(libros);
        }

        public IActionResult Delete(int id)
        {
            _libroService.Detele(id);
            return RedirectToAction(nameof(LibroList));
        }

    }



}