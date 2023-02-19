using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TesteCNPQ.Data;
using TesteCNPQ.Models;

namespace TesteCNPQ.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly ApplicationDbContext Contexto;

        public AgendamentoController(ApplicationDbContext context)
        {
            Contexto = context;
        }

        // GET: Agendamentoes
        public ActionResult Index()
        {
            return View(Contexto.Agendamento.ToList());
        }

        // GET: Agendamentoes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || Contexto.Agendamento == null)
            {
                return NotFound();
            }

            var agendamento = await Contexto.Agendamento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // GET: Agendamentoes/Create
        public IActionResult Create()
        {
            ViewBag.Local = Contexto.Local.ToList();
            return View();
        }

        // POST: Agendamentoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Local,Id,NomeResponsavel,DataInicio,DataTermino")] Agendamento agendamento)
        {
            ViewBag.Local = Contexto.Local.ToList();
            agendamento.Id = Guid.NewGuid();

            if (ModelState.IsValid)
            {
                Contexto.Add(agendamento);
                Contexto.SaveChanges();
                return RedirectToAction("Index", "Home", agendamento);
            }
            throw new Exception("deu pau");
        }

        // GET: Agendamentoes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null || Contexto.Agendamento == null)
            {
                return NotFound();
            }

            var agendamento = Contexto.Agendamento.Find(id);
            if (agendamento == null)
            {
                return NotFound();
            }

            Contexto.SaveChanges();
            return View(agendamento);
        }

        // POST: Agendamentoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,IdLocal,NomeResponsavel,DataInicio,DataTermino")] Agendamento agendamento)
        {
            if (id != agendamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Contexto.Update(agendamento);
                    await Contexto.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgendamentoExists(agendamento.Id))
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
            return View(agendamento);
        }

        // GET: Agendamentoes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || Contexto.Agendamento == null)
            {
                return NotFound();
            }

            var agendamento = await Contexto.Agendamento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // POST: Agendamentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (Contexto.Agendamento == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Agendamento'  is null.");
            }
            var agendamento = await Contexto.Agendamento.FindAsync(id);
            if (agendamento != null)
            {
                Contexto.Agendamento.Remove(agendamento);
            }

            await Contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgendamentoExists(Guid id)
        {
            return Contexto.Agendamento.Any(e => e.Id == id);
        }
    }
}
