using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        // GET: Local
        public ActionResult Index()
        {
            var locais = Contexto.Local.ToList();

            return View("Index", locais);
        }

        // GET: Local/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (Contexto.Local == null)
            {
                return NotFound();
            }

            var local = await Contexto.Local
                .FirstOrDefaultAsync(m => m.Id == id);
            if (local == null)
            {
                return NotFound();
            }

            return View(local);
        }

        // GET: Local/Create
        public ActionResult Create()
        {
            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = "Entrou na tela de Cadastro"
            });

            Contexto.SaveChanges();

            return View();
        }

        // POST: Local/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id, Nome, Endereço, CapacidadeMax")] Local local)
        {
            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = String.Concat("Não sei direito mas o Id é: ",
                local.Id, "e o nome é: ", local.Nome, "e hoje é: ", DateTime.Now.ToLongDateString())
            });

            local.Id = Guid.NewGuid();
            
            Contexto.Local.Add(local);
            Contexto.SaveChanges();

            return View("Index", Contexto.Local.ToList());
        }

        // GET: Local/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || Contexto.Local == null)
            {
                return NotFound();
            }

            var local = await Contexto.Local.FindAsync(id);
            if (local == null)
            {
                return NotFound();
            }

            Contexto.LogAuditoria.Add(new LogAuditoria
            {
                EmailUsuario = User.Identity.Name,
                DetalhesAuditoria = string.Concat("Entrou na tela de Edição ta")
            });

            Contexto.SaveChanges();
            return View(local);
        }

        // POST: Local/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,Endereço,CapacidadeMax")] Local local)
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
                    await Contexto.SaveChangesAsync();

                    Contexto.LogAuditoria.Add(new LogAuditoria
                    {
                        EmailUsuario = User.Identity.Name,
                        DetalhesAuditoria = String.Concat("Não sei direito mas o Id (editado) é: ",
                        local.Id, "e o nome (editado) é: ", local.Nome, "e hoje (edição) é: ", DateTime.Now.ToLongDateString())
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
                return RedirectToAction(nameof(Index));
            }
            return View(local);
        }

        // GET: Local/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null || Contexto.Local == null)
            {
                return NotFound();
            }

            var local = await Contexto.Local
                .FirstOrDefaultAsync(m => m.Id == id);
            if (local == null)
            {
                return NotFound();
            }

            return View(local);
        }

        // POST: Local/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (Contexto.Local == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Local'  is null.");
            }
            var local = await Contexto.Local.FindAsync(id);
            if (local != null)
            {
                Contexto.Local.Remove(local);

                Contexto.LogAuditoria.Add(new LogAuditoria
                {
                    EmailUsuario = User.Identity.Name,
                    DetalhesAuditoria = String.Concat("Não sei direito mas o Id (deletado) é: ",
                    local.Id, "e o nome (deletado) é: ", local.Nome, "e hoje (deleção) é: ", DateTime.Now.ToLongDateString())
                });

                Contexto.SaveChanges();
            }

            await Contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocalExists(Guid id)
        {
            return Contexto.Local.Any(e => e.Id == id);
        }
    }
}
