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

    public class marhalaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: marhala
        public ActionResult Index()
        {
            return View(db.marhalas.ToList());
        }

        // GET: marhala/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            marhala marhala = db.marhalas.Find(id);
            if (marhala == null)
            {
                return HttpNotFound();
            }
            return View(marhala);
        }

        // GET: marhala/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: marhala/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] marhala marhala)
        {
            if (ModelState.IsValid)
            {
                db.marhalas.Add(marhala);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(marhala);
        }

        // GET: marhala/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            marhala marhala = db.marhalas.Find(id);
            if (marhala == null)
            {
                return HttpNotFound();
            }
            return View(marhala);
        }

        // POST: marhala/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] marhala marhala)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marhala).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(marhala);
        }

        // GET: marhala/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            marhala marhala = db.marhalas.Find(id);
            if (marhala == null)
            {
                return HttpNotFound();
            }
            return View(marhala);
        }

        // POST: marhala/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            marhala marhala = db.marhalas.Find(id);
            db.marhalas.Remove(marhala);
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
