using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TesteCNPQ.Data;
using TesteCNPQ.Models;
using DevExpress.Utils.CommonDialogs.Internal;

namespace TesteCNPQ.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly ApplicationDbContext Contexto;

        public AgendamentoController(ApplicationDbContext context)
        {
            Contexto = context;
        }

        public static bool ValidaDataHora(DateTime inicio, DateTime fim)
        {
            if (inicio >= fim)
                return false;

            return true;
        }

        public ActionResult Index()
        {
            var agendamentos = Contexto.Agendamento.OrderBy(a => a.NomeResponsavel).ToList();

            return View(agendamentos);
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null || Contexto.Agendamento == null)
            {
                return NotFound();
            }

            var agendamento = Contexto.Agendamento.FirstOrDefault(m => m.Id == id);

            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = String.Concat("Entrou na tela de Detalhes do Agendamento com Id: ",
                        agendamento.Id, ", responsável: ", agendamento.NomeResponsavel, "na data: ", DateTime.Now.ToLongDateString())
            });

            var local = Contexto.Local.FirstOrDefault(l => l.Id == agendamento.IdLocal);
            agendamento.Local = local;
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        public ActionResult Create()
        {
            ViewBag.Local = Contexto.Local.ToList();

            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = "Entrou na tela de Criação de Agendamento"
            });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,NomeResponsavel,DataInicio,DataTermino")] Agendamento agendamento, IFormCollection formData)
        {
            var locais = Contexto.Local.ToList();
            ViewBag.Local = locais;

            agendamento.Id = Guid.NewGuid();
            var local = formData["Local"].ToString();

            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = String.Concat("Criou um Agendamento com o Id: ",
                agendamento.Id, "e o responsável: ", agendamento.NomeResponsavel, "na data: ", DateTime.Now.ToLongDateString())
            });

            if (local != "Selecione um Local")
            {
                agendamento.Local = locais.FirstOrDefault(l => l.Nome == local);
                agendamento.IdLocal = agendamento.Local.Id;
                agendamento.LocalNome = agendamento.Local.Nome;
            }

            var validaDTHR = ValidaDataHora(agendamento.DataInicio, agendamento.DataTermino);
            if (validaDTHR == false)
                throw new Exception("Data/hora inválido!");
                

            Contexto.Add(agendamento);
            Contexto.SaveChanges();

            return RedirectToAction("Index", "Home", agendamento);

        }

        
        public ActionResult Edit(Guid? id)
        {
            ViewBag.Local = Contexto.Local.ToList();

            if (id == null || Contexto.Agendamento == null)
            {
                return NotFound();
            }

            var agendamento = Contexto.Agendamento.Find(id);

            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = string.Concat("Entrou na tela de Edição do Agendamento com Id:",
                agendamento.Id, "e o responsável: ", agendamento.NomeResponsavel, "na data: ", DateTime.Now.ToLongDateString())
            });

            if (agendamento == null)
            {
                return NotFound();
            }

            Contexto.SaveChanges();
            return View(agendamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,IdLocal,NomeResponsavel,DataInicio,DataTermino")] Agendamento agendamento, IFormCollection formData)
        {

            var locais = Contexto.Local.ToList();
            ViewBag.Local = locais;

            var local = formData["Local"].ToString();

            agendamento.Local = locais.FirstOrDefault(l => l.Nome == local);
            agendamento.IdLocal = agendamento.Local.Id;

            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = String.Concat("Editou o Agendamento com o Id: ",
                        agendamento.Id, ", com o responsávele: ", agendamento.NomeResponsavel, "na data: ", DateTime.Now.ToLongDateString())
            });

            Contexto.Update(agendamento);
            Contexto.SaveChanges();


            return RedirectToAction("Index", "Home", agendamento);
        }

        public ActionResult Delete(Guid id)
        {
            var agendamento = Contexto.Agendamento.Find(id);

            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = String.Concat("Removeu o Agendamento do Id: ",
                        agendamento.Id, ", com o responsável: ", agendamento.NomeResponsavel, "na data: ", DateTime.Now.ToLongDateString())
            });

            if (agendamento != null)
            {
                Contexto.Agendamento.Remove(agendamento);
            }

            Contexto.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        private bool AgendamentoExists(Guid id)
        {
            return Contexto.Agendamento.Any(e => e.Id == id);
        }
    }
}
