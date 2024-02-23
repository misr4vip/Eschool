using ESchool.Models;
using LinqToExcel;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static ESchool.Models.ApplicationUser;

namespace ESchool.Controllers
{
    [Authorize]


    public class ExcelFilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ExcelFilesController()
        {

        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult UploadStudentExcel()
        {
            ViewBag.marhalaId = new SelectList(db.marhalas, "Id", "Name");
            ViewBag.SheetCount = 2;
            return View();
        }

            [HttpPost]
        public  ActionResult UploadStudentExcel(HttpPostedFileBase FileUpload,int marhalaId,int SaufId,int ClassId,int SheetCount)
        {
            List<string> data = new List<string>();

            try
            {
                if (FileUpload != null)
                {
                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");
                    if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        string filename = FileUpload.FileName;
                        string targetpath = Server.MapPath("~/Doc/");
                        FileUpload.SaveAs(targetpath + filename);
                        string pathToExcelFile = targetpath + filename;
                        var connectionString = "";
                        if (filename.EndsWith(".xls"))
                        {
                            connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                        }
                        else if (filename.EndsWith(".xlsx"))
                        {
                            connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                        }
                        for (int index = 1; index <= SheetCount; index++)
                        {
                            string sheetName = "Sheet"+ index;
                            var adapter = new OleDbDataAdapter("SELECT * FROM ["+sheetName+"$]", connectionString);
                            var ds = new DataSet();
                            adapter.Fill(ds, "ExcelTable");
                            DataTable dtable = ds.Tables["ExcelTable"];
                            
                            var excelFile = new ExcelQueryFactory(pathToExcelFile);
                            var Students = from a in excelFile.Worksheet(sheetName) select a;
                            var al = Students.ToList();
                            var classRoom = db.ClassRooms.Find(ClassId);
                            if (classRoom != null && classRoom.Name.Equals("مقررات"))
                            {
                                for (int a = 17; a < al.Count(); a = a + 2)
                                {
                                    try
                                    {
                                        ApplicationUser member = new ApplicationUser();
                                        switch (al[a][21])
                                        {
                                            case "السعودية":
                                                member.nathionality = Nathionality.السعودية;
                                                break;
                                            case "الأردن":
                                                member.nathionality = Nathionality.الأردن;
                                                break;
                                            case "سوريا":
                                                member.nathionality = Nathionality.سوريا;
                                                break;
                                            case "مصر":
                                                member.nathionality = Nathionality.مصر;
                                                break;
                                            case "اليمن":
                                                member.nathionality = Nathionality.اليمن;
                                                break;
                                            case "عمان":
                                                member.nathionality = Nathionality.عمان;
                                                break;
                                            case "فلسطين":
                                                member.nathionality = Nathionality.فلسطين;
                                                break;
                                            case "قطر":
                                                member.nathionality = Nathionality.قطر;
                                                break;
                                            case "لبنان":
                                                member.nathionality = Nathionality.لبنان;
                                                break;
                                            default:
                                                member.nathionality = Nathionality.الهند;
                                                break;
                                        }

                                        member.Dob = al[a + 1][9];
                                        member.IdentityId = al[a][14];
                                        member.Name = al[a][23];
                                        member.EnglishName = al[a + 1][23];
                                        member.type = MemberType.Student;
                                        member.UserName = member.IdentityId;
                                        member.Email = "";
                                        member.EmailConfirmed = false;
                                        member.PhoneNumberConfirmed = false;
                                        member.TwoFactorEnabled = false;
                                        member.status = MemberStatus.نشط;
                                        db.Users.Add(member);
                                        db.SaveChanges();
                                        //// add student to class
                                        MemberClassYear classYear = new MemberClassYear();
                                        classYear.marhalaId = marhalaId;
                                        classYear.SaufId = SaufId;
                                        classYear.ClassId = ClassId;
                                        classYear.MemberId = member.Id;
                                        classYear.IsCurrent = true;
                                        Setting setting = db.Settings.Where(s => s.IsCurrent).FirstOrDefault();
                                        classYear.Year = Convert.ToInt32(setting.yearOfStudy);

                                        db.MemberClassYears.Add(classYear);
                                        db.SaveChanges();
                                    }
                                    catch (DbEntityValidationException ex)
                                    {
                                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                        {
                                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                                            {
                                                data.Add("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                            }
                                        }
                                        data.ToArray();
                                        return Json(data, JsonRequestBehavior.AllowGet);

                                    }
                                }

                            }
                            else
                            {
                            for (int a = 24; a < al.Count(); a = a + 2)
                            {
                                try
                                {
                                    ApplicationUser member = new ApplicationUser();
                                    switch (al[a][33])
                                    {
                                        case "السعودية":
                                            member.nathionality = Nathionality.السعودية;
                                            break;
                                        case "الأردن":
                                            member.nathionality = Nathionality.الأردن;
                                            break;
                                        case "سوريا":
                                            member.nathionality = Nathionality.سوريا;
                                            break;
                                        case "مصر":
                                            member.nathionality = Nathionality.مصر;
                                            break;
                                        case "اليمن":
                                            member.nathionality = Nathionality.اليمن;
                                            break;
                                        case "عمان":
                                            member.nathionality = Nathionality.عمان;
                                            break;
                                        case "فلسطين":
                                            member.nathionality = Nathionality.فلسطين;
                                            break;
                                        case "قطر":
                                            member.nathionality = Nathionality.قطر;
                                            break;
                                        case "لبنان":
                                            member.nathionality = Nathionality.لبنان;
                                            break;
                                        default:
                                            member.nathionality = Nathionality.الهند;
                                            break;
                                    }

                                    member.Dob = al[a+1][18];
                                    member.IdentityId = al[a][28];
                                    member.Name = al[a][35];
                                    member.EnglishName = al[a + 1][35];
                                    member.type = MemberType.Student;
                                    member.UserName = member.IdentityId;
                                    member.Email = "";
                                    member.EmailConfirmed = false;
                                    member.PhoneNumberConfirmed = false;
                                    member.TwoFactorEnabled = false;
                                    member.status = MemberStatus.نشط;
                                    db.Users.Add(member);
                                    db.SaveChanges();
                                     //// add student to class
                                     MemberClassYear classYear = new MemberClassYear();
                                    classYear.marhalaId = marhalaId;
                                    classYear.SaufId = SaufId;
                                    classYear.ClassId = ClassId;
                                    classYear.MemberId = member.Id;
                                    classYear.IsCurrent = true;
                                    Setting setting = db.Settings.Where(s => s.IsCurrent).FirstOrDefault();
                                    classYear.Year = Convert.ToInt32(setting.yearOfStudy);

                                    db.MemberClassYears.Add(classYear);
                                    db.SaveChanges();
                                }
                                catch (DbEntityValidationException ex)
                                {
                                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                    {
                                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                                        {
                                            data.Add("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                        }
                                    }
                                    data.ToArray();
                                    return Json(data, JsonRequestBehavior.AllowGet);

                                }
                            }

                            }
                           
                            //deleting excel file from folder
                           

                        }
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        return RedirectToAction("index", "Account");
                    }
                    else
                    {
                        //alert message for invalid file format
                        data.Add("<ul>");
                        data.Add("<li>Only Excel file format is allowed</li>");
                        data.Add("</ul>");
                        data.ToArray();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    data.Add("<ul>");
                    if (FileUpload == null) data.Add("<li>Please choose Excel file</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

            }
            catch (SystemException sysException) {

                Console.WriteLine(sysException.Message) ;
                data.Add("<ul>");
                data.Add("<li>" + sysException.Message + " " + sysException.InnerException + "</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception x)
            {

                data.Add("<ul>");
                data.Add("<li>"+x.Message +" " + x.InnerException+"</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
    }
}