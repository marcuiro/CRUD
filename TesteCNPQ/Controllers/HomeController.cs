using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TesteCNPQ.Data;
using TesteCNPQ.Models;

namespace TesteCNPQ.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly ApplicationDbContext Contexto;

        public HomeController(ApplicationDbContext context)
        {
            Contexto = context;
        }

        public ActionResult Index()
        {
            ViewBag.Local = Contexto.Local.ToList();

            var agendamentos = Contexto.Agendamento.ToList();
            return View(agendamentos);
        }

        public ActionResult ListaLocais()
        {
            var locais = Contexto.Local.ToList();
            return View(locais);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}