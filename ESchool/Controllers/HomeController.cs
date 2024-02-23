using ESchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESchool.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            int superAdmin = 0, Admin = 0, Account = 0, Student = 0, Teacher = 0, SuperTeacher = 0, Leader = 0;
            var users = db.Users.ToList();
            foreach (var user in users)
            {
                switch (user.type)
                {
                    case ApplicationUser.MemberType.superAdmin:
                        superAdmin++;
                        break;
                    case ApplicationUser.MemberType.Admin:
                        Admin++;
                        break;
                    case ApplicationUser.MemberType.Student:
                        Student++;
                        break;
                    case ApplicationUser.MemberType.Account:
                        Account++;
                        break;
                    case ApplicationUser.MemberType.Teacher:
                        Teacher++;
                        break;
                    case ApplicationUser.MemberType.SuperTeacher:
                        SuperTeacher++;
                        break;
                    case ApplicationUser.MemberType.Leader:
                        Leader++;
                        break;
                    
                }
            }
            ViewBag.superAdmin = superAdmin;
            ViewBag.Admin = Admin;
            ViewBag.Student = Student;
            ViewBag.Account = Account;
            ViewBag.Teacher = Teacher;
            ViewBag.SuperTeacher = SuperTeacher;
            ViewBag.Leader = Leader;
            var Motaser = 0;
            var notMotaser = 0;
            var members = db.Users.ToList();
            foreach (var item in members)
            {
                if (item.type == ApplicationUser.MemberType.Student)
                {
                    if (item.expenses.ToList().Sum(e => e.ExpensesValue) > item.disposals.ToList().Sum(d => d.Amount))
                    {
                        Motaser++;
                    }
                    else
                    {
                        notMotaser++;
                    }
                }
               
            }
            ViewBag.Motaser = Motaser;
            ViewBag.notMotaser = notMotaser;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}