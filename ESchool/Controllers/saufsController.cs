using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ESchool.Models;

namespace ESchool.Controllers
{
    [Authorize]

    public class saufsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: saufs
        public ActionResult Index()
        {
            var saufs = db.saufs.Include(s => s.marhala);
            return View(saufs.ToList());
        }

        // GET: saufs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sauf sauf = db.saufs.Find(id);
            if (sauf == null)
            {
                return HttpNotFound();
            }
            return View(sauf);
        }

        // GET: saufs/Create
        public ActionResult Create()
        {
            ViewBag.marhalaId = new SelectList(db.marhalas, "Id", "Name");
            return View();
        }

        // POST: saufs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,marhalaId")] sauf sauf)
        {
            if (ModelState.IsValid)
            {
                db.saufs.Add(sauf);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.marhalaId = new SelectList(db.marhalas, "Id", "Name", sauf.marhalaId);
            return View(sauf);
        }

        // GET: saufs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sauf sauf = db.saufs.Find(id);
            if (sauf == null)
            {
                return HttpNotFound();
            }
            ViewBag.marhalaId = new SelectList(db.marhalas, "Id", "Name", sauf.marhalaId);
            return View(sauf);
        }

        // POST: saufs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,marhalaId")] sauf sauf)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sauf).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.marhalaId = new SelectList(db.marhalas, "Id", "Name", sauf.marhalaId);
            return View(sauf);
        }

        // GET: saufs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sauf sauf = db.saufs.Find(id);
            if (sauf == null)
            {
                return HttpNotFound();
            }
            return View(sauf);
        }

        // POST: saufs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sauf sauf = db.saufs.Find(id);
            db.saufs.Remove(sauf);
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
