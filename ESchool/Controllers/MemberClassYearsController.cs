using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ESchool.Models;

namespace ESchool.Controllers
{
    [Authorize]

    public class MemberClassYearsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MemberClassYears
        public ActionResult Index(string memberId)
        {
            List<MemberClassYear> memberClasses;
            if (memberId != null)
            {
                memberClasses = db.MemberClassYears.Where(m => m.MemberId == memberId).ToList();
            }
            else
            {
                memberClasses = db.MemberClassYears.ToList();

            }
            ViewBag.MemberId = memberId;
            return View(memberClasses);
        }

        public JsonResult GetSauf(int marhalaId)
        {
            var Saufs = db.saufs.Where(s => s.marhalaId == marhalaId).ToList();

            return Json(Saufs, JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult GetClass(int SaufId)
        {
            var Classes = db.ClassRooms.Where(s => s.saufId == SaufId).ToList();

            return Json(Classes, JsonRequestBehavior.AllowGet);

        }
        // GET: MemberClassYears/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberClassYear memberClassYear = db.MemberClassYears.Find(id);
            if (memberClassYear == null)
            {
                return HttpNotFound();
            }
            return View(memberClassYear);
        }

        // GET: MemberClassYears/Create
        public ActionResult Create(string memberId)
        {
            ViewBag.MemberId = new SelectList(db.Users.Where(m=>m.Id == memberId), "Id", "Name");
            ViewBag.marhalaId = new SelectList(db.marhalas, "Id", "Name");
            return View();
        }

        // POST: MemberClassYears/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create( MemberClassYear memberClassYear)
        {
            if (ModelState.IsValid)
            {
                if (memberClassYear.IsCurrent)
                {
                    db.MemberClassYears.Where(m => m.MemberId == memberClassYear.MemberId).ForEachAsync(m => m.IsCurrent = false);
                }
                MemberExpenses expenses = new MemberExpenses();
                var setting = db.Settings.Where(s=>s.IsCurrent).ToList().FirstOrDefault();
                var marhala = db.marhalas.Find(memberClassYear.marhalaId);
                var member = db.Users.Find(memberClassYear.MemberId);
                expenses.YearOfStudy = setting.yearOfStudy;
                expenses.Semster = setting.semster.ToString();
                expenses.expensesText = MemberExpenses.ExpensesType.رسوم_الفصل_الدراسي;
                memberClassYear.Year =Convert.ToInt32( setting.yearOfStudy);
                if (marhala != null)
                {
                    if (marhala.Name.Contains("بتدائي"))
                    {
                        expenses.ExpensesValue = Convert.ToDouble(setting.Primary);
                    }
                    else if (marhala.Name.Contains("متوسط"))
                    {
                        expenses.ExpensesValue = Convert.ToDouble(setting.Intermediate);
                    }
                    else if (marhala.Name.Contains("ثانو"))
                    {
                        expenses.ExpensesValue = Convert.ToDouble(setting.Secondary);
                    }
                }
                expenses.userId = memberClassYear.MemberId;
            
                    expenses.TotalExpensesValue = expenses.ExpensesValue;
                db.MemberExpenses.Add(expenses);
                db.MemberClassYears.Add(memberClassYear);
                db.SaveChanges();
                
                return RedirectToAction("Index",new { memberId = memberClassYear.MemberId });
            }

            return View(memberClassYear);
        }

        // GET: MemberClassYears/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberClassYear memberClassYear = db.MemberClassYears.Find(id);
            if (memberClassYear == null)
            {
                return HttpNotFound();
            }
            return View(memberClassYear);
        }

        // POST: MemberClassYears/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MemberId,Year,marhalaId,SaufId,ClassId,IsCurrent")] MemberClassYear memberClassYear)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memberClassYear).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(memberClassYear);
        }

        // GET: MemberClassYears/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberClassYear memberClassYear = db.MemberClassYears.Find(id);
            if (memberClassYear == null)
            {
                return HttpNotFound();
            }
            return View(memberClassYear);
        }

        // POST: MemberClassYears/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MemberClassYear memberClassYear = db.MemberClassYears.Find(id);
            db.MemberClassYears.Remove(memberClassYear);
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
