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
    public class ProgramadoresController : Controller
    {
        private PBLVIIContext db = new PBLVIIContext();

        // GET: Programadores
        public ActionResult Index()
        {
            return View(db.Programadors.ToList());
        }
   
      public ActionResult RelatorioAtividadesPorProgramador(int id, int? pagina, Boolean? geraPDF)
      {
         var tarefas = db.Tarefas.Where(p => p.ProgramadorId == id ).ToList();
        

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
               ViewName = "RelatorioAtividadesPorProgramador",
               PageSize = Size.A4,
               IsGrayScale = false,
               Model = tarefas.ToPagedList(paginaNumero, tarefas.Count)
            };
            return pdf;
         }
         
      }


      // GET: Programadores/Details/5
      public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programador programador = db.Programadors.Find(id);
            if (programador == null)
            {
                return HttpNotFound();
            }
            return View(programador);
        }

        // GET: Programadores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Programadores/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProgramadorId,Nome,Nivel")] Programador programador)
        {
            if (ModelState.IsValid)
            {
                db.Programadors.Add(programador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(programador);
        }

        // GET: Programadores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programador programador = db.Programadors.Find(id);
            if (programador == null)
            {
                return HttpNotFound();
            }
            return View(programador);
        }

        // POST: Programadores/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProgramadorId,Nome,Nivel")] Programador programador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(programador);
        }

        // GET: Programadores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programador programador = db.Programadors.Find(id);
            if (programador == null)
            {
                return HttpNotFound();
            }
            return View(programador);
        }

        // POST: Programadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Programador programador = db.Programadors.Find(id);
            db.Programadors.Remove(programador);
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
