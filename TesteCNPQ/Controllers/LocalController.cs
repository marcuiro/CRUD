using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TesteCNPQ.Data;
using TesteCNPQ.Models;

namespace TesteCNPQ.Controllers
{
    [Authorize]
    public class LocalController : Controller
    {
        private readonly ApplicationDbContext Contexto;

        public LocalController(ApplicationDbContext context)
        {
            Contexto = context;
        }

        public ActionResult Index()
        {
            var locais = Contexto.Local.ToList();

            return View("Index", locais);
        }

        public ActionResult Details(Guid id)
        {
            if (Contexto.Local == null)
            {
                return NotFound();
            }

            var local = Contexto.Local.FirstOrDefault(m => m.Id == id);

            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = String.Concat("Entrou na tela de Detalhes do Local com Id: ",
                        local.Id, ", nome: ", local.Nome, "na data: ", DateTime.Now.ToLongDateString())
            });

            if (local == null)
            {
                return NotFound();
            }

            return View(local);
        }

        
        public ActionResult Create()
        {
            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = "Entrou na tela de Criação de Local"
            });

            Contexto.SaveChanges();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id, Nome, Endereço, CapacidadeMax")] Local local)
        {
            local.Id = Guid.NewGuid();

            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = String.Concat("Criou um Local com o Id: ",
                local.Id, "e o nome: ", local.Nome, "na data: ", DateTime.Now.ToLongDateString())
            });

            Contexto.Local.Add(local);
            Contexto.SaveChanges();

            return RedirectToAction("Index", "Home", Contexto.Local.ToList());
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null || Contexto.Local == null)
            {
                return NotFound();
            }

            var local = Contexto.Local.Find(id);
            if (local == null)
            {
                return NotFound();
            }

            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = string.Concat("Entrou na tela de Edição do Local com Id:",
                local.Id, "e o nome: ", local.Nome, "na data: ", DateTime.Now.ToLongDateString())
            });

            Contexto.SaveChanges();
            return View(local);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("Id,Nome,Endereço,CapacidadeMax")] Local local)
        {
            if (id != local.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Contexto.Update(local);
                    Contexto.SaveChanges();

                    Contexto.LogAuditoria.Add(new LogAuditoria
                    {
                        EmailUsuario = User.Identity.Name,
                        DetalhesAuditoria = String.Concat("Editou o Local com o Id: ",
                        local.Id, ", com o novo nome: ", local.Nome, "na data: ", DateTime.Now.ToLongDateString())
                    });

                    Contexto.SaveChanges();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalExists(local.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(local);
        }

        public ActionResult Delete(Guid id)
        {
            if (Contexto.Local == null)
            {
                return NotFound();
            }
            var local = Contexto.Local.FirstOrDefault(m => m.Id == id);

            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = String.Concat("Removeu o Local do Id: ",
                        local.Id, ", com o nome: ", local.Nome, "na data: ", DateTime.Now.ToLongDateString())
            });
            
            if (local == null)
            {
                return NotFound();
            }
            Contexto.Remove(local);
            Contexto.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        private bool LocalExists(Guid id)
        {
            return Contexto.Local.Any(e => e.Id == id);
        }
    }
}
