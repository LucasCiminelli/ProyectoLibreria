using AppStore.Models.Domain;
using AppStore.Models.DTO;
using AppStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace AppStore.Controllers;

public class HomeController : Controller
{

    private readonly ILibroService? _libroService;


    public HomeController(ILibroService libroService)
    {
        _libroService = libroService;
    }

    public IActionResult Index(string term = "", int currentPage = 1)
    {
        LibroListVm librosListVm = _libroService!.List(term, true, currentPage);

        return View(librosListVm);
    }


    public IActionResult LibroDetail(int libroId)
    {

        Libro libroBuscado = _libroService!.GetById(libroId);

        return View(libroBuscado);
    }


    public IActionResult About()
    {

        return View();
    }

}