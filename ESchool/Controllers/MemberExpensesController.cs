using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ESchool.Models;
using static ESchool.Models.ApplicationUser;

namespace ESchool.Controllers
{
    [Authorize]

    public class MemberExpensesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MemberExpenses
        public ActionResult Index(string memberId)
        {
            List<MemberExpenses> memberExpenses;
            if (memberId != null)
            {
                memberExpenses = db.MemberExpenses.Include(m => m.user).Where(m => m.userId == memberId).ToList();

            }
            else
            {
                memberExpenses = db.MemberExpenses.Include(m => m.user).ToList();

            }
            return View(memberExpenses);
        }
       
        public ActionResult AddStudyCostToAll()
        {
            var ClassYear = db.MemberClassYears.Where(m => m.IsCurrent).ToList();
            var setting = db.Settings.Where(s => s.IsCurrent).ToList().FirstOrDefault();
            try
            {
                foreach (var item in ClassYear)
                {
                    var member = db.Users.Find(item.MemberId);
                    if (member != null && member.status == ApplicationUser.MemberStatus.نشط)
                    {
                        var marhala = db.marhalas.Find(item.marhalaId);

                        var expences = new MemberExpenses();
                        expences.YearOfStudy = item.Year.ToString();
                        expences.expensesText = MemberExpenses.ExpensesType.رسوم_الفصل_الدراسي;
                        expences.userId = item.MemberId;
                        expences.Semster = setting.semster.ToString();
                        if (marhala.Name.Contains("بتدائي"))
                        {
                            expences.ExpensesValue = Convert.ToDouble(setting.Primary);
                        }
                        else if (marhala.Name.Contains("متوسط"))
                        {
                            expences.ExpensesValue = Convert.ToDouble(setting.Intermediate);
                        }
                        else if (marhala.Name.Contains("ثانو"))
                        {
                            expences.ExpensesValue = Convert.ToDouble(setting.Secondary);
                        }



                        if (db.Users.Find(item.MemberId).nathionality == Nathionality.السعودية)
                        {
                            expences.Vat = 0;
                        }
                        else
                        {
                            expences.Vat = (setting.Vat / 100) * expences.ExpensesValue;
                        }

                        expences.Discount = 0;
                        expences.TotalExpensesValue = expences.ExpensesValue + expences.Vat - expences.Discount;
                        expences.Semster = setting.semster.ToString();

                        db.MemberExpenses.Add(expences);
                    }
                   

                }

            }
            catch (Exception)
            {

                throw;
            }
            db.SaveChanges();

            return RedirectToAction("index");
        }
        // GET: MemberExpenses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberExpenses memberExpenses = db.MemberExpenses.Include(m=>m.user).Where(m=>m.Id == id).ToList().FirstOrDefault();
            if (memberExpenses == null)
            {
                return HttpNotFound();
            }
            return View(memberExpenses);
        }

        // GET: MemberExpenses/Create
        public ActionResult Create(string memberId)
        {

            if (memberId != null)
            {
                ViewBag.memberDataId = new SelectList(db.Users.Where(m=>m.Id == memberId), "Id", "Name");

            }
            else
            {
                ViewBag.memberDataId = new SelectList(db.Users, "Id", "Name");

            }
            return View();
        }

        // POST: MemberExpenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( MemberExpenses memberExpenses)
        {
            if (ModelState.IsValid)
            {
                db.MemberExpenses.Add(memberExpenses);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.memberDataId = new SelectList(db.Users, "Id", "Name", memberExpenses.userId);
            return View(memberExpenses);
        }

        // GET: MemberExpenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
              //  ViewBag.memberDataId = new SelectList(db.MemberDatas.Where(m => m.Id == id), "Id", "Name");

           
            MemberExpenses memberExpenses = db.MemberExpenses.Find(id);
            if (memberExpenses == null)
            {
                return HttpNotFound();
            }
           
            ViewBag.userId = new SelectList(db.Users.Where(m => m.Id == memberExpenses.userId), "Id", "Name", memberExpenses.userId);
            return View(memberExpenses);
        }

        // POST: MemberExpenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( MemberExpenses memberExpenses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memberExpenses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userId = new SelectList(db.Users, "Id", "IdentityId", memberExpenses.userId);
            return View(memberExpenses);
        }

        // GET: MemberExpenses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberExpenses memberExpenses = db.MemberExpenses.Include(m => m.user).Where(m => m.Id == id).ToList().FirstOrDefault();
            if (memberExpenses == null)
            {
                return HttpNotFound();
            }
            return View(memberExpenses);
        }

        // POST: MemberExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MemberExpenses memberExpenses = db.MemberExpenses.Find(id);
            db.MemberExpenses.Remove(memberExpenses);
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
