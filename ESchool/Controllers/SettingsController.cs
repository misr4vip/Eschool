﻿using System;
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

    public class SettingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        public void ChangeState(int id)
        {
            var settings = db.Settings.ToList();
            foreach (var item in settings)
            {
                if (item.Id == id)
                {
                    item.IsCurrent = true;
                }
                else
                {
                    item.IsCurrent = false;
                }
                db.Entry(item).State = EntityState.Modified;

                db.SaveChanges();
                
            }
        }
        // GET: Settings
        public ActionResult Index()
        {
            return View(db.Settings.ToList());
        }

        // GET: Settings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = db.Settings.Find(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // GET: Settings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Settings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Setting setting)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if (setting.IsCurrent)
                    //{
                    //    var allSettings = db.Settings.ToList();
                    //    foreach (var item in allSettings)
                    //    {
                    //        item.IsCurrent = false;
                    //        db.Entry(item).State = EntityState.Modified;
                    //        db.SaveChanges();
                    //    }
                    //}

                    ChangeState(setting.Id);
                }
                catch (Exception)
                {

                   
                }
               
                db.Settings.Add(setting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(setting);
        }

        // GET: Settings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = db.Settings.Find(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: Settings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Setting setting)
        {
            if (ModelState.IsValid)
            {
                ChangeState(setting.Id);
                //db.Entry(setting).State = EntityState.Unchanged;
                //db.Entry(setting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(setting);
        }

        // GET: Settings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = db.Settings.Find(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Setting setting = db.Settings.Find(id);
            db.Settings.Remove(setting);
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
