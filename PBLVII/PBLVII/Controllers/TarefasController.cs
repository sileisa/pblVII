using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PBLVII.Models;
using Rotativa;
using Rotativa.Options;

namespace PBLVII.Controllers
{
   public class TarefasController : Controller
   {
      private PBLVIIContext db = new PBLVIIContext();


      public ActionResult Index()
      {
         var tarefas = db.Tarefas.Include(t => t.Programador);
         return View(tarefas.ToList());
      }

      public ActionResult BuscarTarefaPorPeriodo()
      {
         return View();

      }

      [HttpPost]
      public ActionResult BuscarTarefaPorPeriodo(DateTime dataInicio, DateTime dataFim, int? pagina, Boolean? geraPDF)
      {
         var tarefas = db.Tarefas.Where(i => i.Status == "Done" && i.DataInicio >= dataInicio && i.DataFim <= dataFim).ToList();
         List<Tarefa> tarefass = new List<Tarefa>();
         foreach (var item in tarefas)
         {
            db.Entry(item).Reference(t => t.Programador).Load();
            tarefass.Add(item);
         }
         if (geraPDF == true)
         {
            int pgQtdRegistro = 2;
            int pgNav = (pagina ?? 1);

            return View(tarefas.ToPagedList(pgNav, pgQtdRegistro));
         }
         else
         {
            int paginaNumero = 1;
            var pdf = new ViewAsPdf
            {
               ViewName = "RelatorioTarefasPorPeriodo",
               PageSize = Size.A4,
               IsGrayScale = false,
               Model = tarefas.ToPagedList(paginaNumero, tarefas.Count)
            };
            return pdf;
         }

      }

      // GET: Tarefas/Details/5
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Tarefa tarefa = db.Tarefas.Find(id);
         if (tarefa == null)
         {
            return HttpNotFound();
         }
         return View(tarefa);
      }

      // GET: Tarefas/Create
      public ActionResult Create()
      {
         ViewBag.ProgramadorId = new SelectList(db.Programadors, "ProgramadorId", "Nome");
         return View();
      }

      // POST: Tarefas/Create
      // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
      // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "TarefaId,Nome,Status,DataInicio,DataFim,ProgramadorId")] Tarefa tarefa)
      {
         if (ModelState.IsValid)
         {
            db.Tarefas.Add(tarefa);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.ProgramadorId = new SelectList(db.Programadors, "ProgramadorId", "Nome", tarefa.ProgramadorId);
         return View(tarefa);
      }

      // GET: Tarefas/Edit/5
      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Tarefa tarefa = db.Tarefas.Find(id);
         if (tarefa == null)
         {
            return HttpNotFound();
         }
         ViewBag.ProgramadorId = new SelectList(db.Programadors, "ProgramadorId", "Nome", tarefa.ProgramadorId);
         return View(tarefa);
      }

      // POST: Tarefas/Edit/5
      // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
      // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "TarefaId,Nome,Status,DataInicio,DataFim,ProgramadorId")] Tarefa tarefa)
      {
         if (ModelState.IsValid)
         {
            db.Entry(tarefa).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.ProgramadorId = new SelectList(db.Programadors, "ProgramadorId", "Nome", tarefa.ProgramadorId);
         return View(tarefa);
      }

      // GET: Tarefas/Delete/5
      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Tarefa tarefa = db.Tarefas.Find(id);
         if (tarefa == null)
         {
            return HttpNotFound();
         }
         return View(tarefa);
      }

      // POST: Tarefas/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         Tarefa tarefa = db.Tarefas.Find(id);
         db.Tarefas.Remove(tarefa);
         db.SaveChanges();
         return RedirectToAction("Index");
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            db.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}
