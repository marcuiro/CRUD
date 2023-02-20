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

        public DateTime ConvertToDateTime(string date)
        {
            DateTime data;

            bool isDate = DateTime.TryParse(date, out data);

            return data;
        }

        public ActionResult Index(IFormCollection formData)
        {
            var pesquisaLocal = formData["pesquisaLocal"].ToString();
            var pesquisaAgendamento = formData["pesquisaAgendamento"].ToString();
            var tipoFiltroLocal = formData["tipoFiltroLocal"].ToString();
            var tipoFiltroAgendamento = formData["tipoFiltroAgendamento"].ToString();
            var inicio = formData["inicio"].ToString();
            var fim = formData["fim"].ToString();
            var agendamentos = Contexto.Agendamento.ToList();


            if (pesquisaLocal != "")
                ViewBag.Local = Contexto.Local.Where(l => (tipoFiltroLocal == "Nome" && l.Nome.Contains(pesquisaLocal)) ||
                                                          (tipoFiltroLocal == "Endereço" && l.Endereço.Contains(pesquisaLocal)) ||
                                                          (tipoFiltroLocal == "Cap. Máxima" && l.CapacidadeMax.Contains(pesquisaLocal)))
                                                          .OrderBy(l => l.Nome)
                                                          .ToList();
            else
                ViewBag.Local = Contexto.Local.OrderBy(l => l.Nome).ToList();
            
            if (pesquisaAgendamento != "" || inicio != "" || fim != "")
                agendamentos = Contexto.Agendamento.Where(a => (tipoFiltroAgendamento == "Responsável" && a.NomeResponsavel.Contains(pesquisaAgendamento)) ||
                                                               (tipoFiltroAgendamento == "Local" && a.LocalNome.Contains(pesquisaAgendamento)) ||
                                                               (tipoFiltroAgendamento == "Data" && ((this.ConvertToDateTime(inicio) == a.DataInicio) ||
                                                               (this.ConvertToDateTime(fim)) == a.DataTermino)))
                                                               .OrderBy(a => a.NomeResponsavel)
                                                               .ToList();
            else
                agendamentos = Contexto.Agendamento.OrderBy(a => a.NomeResponsavel).ToList();

            
            return View("Index", agendamentos);
        }

        public ActionResult ListaLocais()
        {
            var locais = Contexto.Local.ToList();
            return View(locais);
        }

        public ActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}