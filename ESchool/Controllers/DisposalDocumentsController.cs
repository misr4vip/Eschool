using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using ESchool.Models;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using QRCoder;
using Rotativa.MVC;
using static ESchool.Models.ApplicationUser;

namespace ESchool.Controllers
{
    

    public class DisposalDocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //public string NumberConverter(double num)
        //{
        //    N2C.ConvertN2C.ConvertNow(num,)
        //}
        // GET: DisposalDocuments
        public ActionResult Index()
        {
            var disposalDocuments = db.DisposalDocuments.Include(d => d.user);
            return View(disposalDocuments.ToList());
        }
        public ActionResult IndexById(string memberId)
        {
            if (memberId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var disposalDocuments = db.DisposalDocuments.Include(d => d.user).Where(d=>d.userId == memberId);
            return View(disposalDocuments.ToList());
        }
        // GET: DisposalDocuments/Details/5
        public ActionResult Details(int? id )
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisposalDocument disposalDocument = db.DisposalDocuments.Include(d=>d.user).Where(d=>d.Id == id).ToList().FirstOrDefault();
         
            if (disposalDocument == null)
            {
                return HttpNotFound();
            }
            return View(disposalDocument);
        }

        // GET: DisposalDocuments/Create
        public ActionResult Create(string memberId)
        {
            if (memberId != null)
            {
                var Member = db.Users.Where(m => m.Id == memberId && m.type == MemberType.Student).ToList();
                if (Member != null)
                {
                    ViewBag.userId = new SelectList(Member, "Id", "Name");

                    if (Member.FirstOrDefault().nathionality == Nathionality.السعودية)
                    {
                        ViewBag.nathionality = "سعودي";
                    }
                    else
                    {
                        ViewBag.nathionality = "مقيم";
                    }
                }
                

            }
            else
            {
                ViewBag.userId = new SelectList(db.Users.Where( m=>m.type == MemberType.Student), "Id", "Name");

            }
            return View();
        }

        // POST: DisposalDocuments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( DisposalDocument disposalDocument)
        {
            var condition = false;
            if (ModelState.IsValid)
            {

                var member = db.Users.Find(disposalDocument.userId);
                if (member.nathionality == Nathionality.السعودية && disposalDocument.Vat == 0)
                {
                    condition = true;
                }
                else if (member.nathionality != Nathionality.السعودية && disposalDocument.Vat > 0 )
                {
                    condition = true;
                }
                if (condition)
                {
                    db.DisposalDocuments.Add(disposalDocument);
                    db.SaveChanges();
                    return RedirectToAction("DisposalDocument","Report", new { id = disposalDocument.Id });
                }
                else
                {
                    ModelState.AddModelError("", "عفوا ضريبة القيمة المضافة لغير الجنسية السعودية");
                }
               
            }
            ViewBag.memberDataId = new SelectList(db.Users, "Id", "Name", disposalDocument.userId);
            return View(disposalDocument);
        }

        //public ActionResult PrintDocumnetById(int id)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    PdfWriter pw = new PdfWriter(ms);
        //    PdfDocument pdfDocument = new PdfDocument(pw);
        //    Document doc = new Document(pdfDocument, PageSize.A7);
        //    Paragraph p1 = new Paragraph();

        //    p1.Add(string.Format(" سند رقم {0}", id));
        //    Style style1 = new Style();
        //    PdfFont font = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.TIMES_ROMAN);
        //    style1.SetFont(font);
        //    style1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //    p1.AddStyle(style1);


        //    doc.Add(p1);
        //    doc.Close();
        //    byte[] byteStream = ms.ToArray();
        //    ms = new MemoryStream();
        //    ms.Write(byteStream, 0, byteStream.Length);
        //    ms.Position = 0;
        //    return new FileStreamResult(ms, "application/pdf");

        //}
        public ActionResult Download_PDF(int id)
        {
            ReportDocument rd = new ReportDocument();
            // rd.Load(Path.Combine(Server.MapPath("~/Reports/SalesdetailsFromToRpt.rpt")));
            rd.Load("~/Reports/DisposalDocumentRPT.rpt");
            DataTable DisposalDocument = new DataTable("DisposalDocument");

            DisposalDocument.Columns.Add("Id");
            DisposalDocument.Columns.Add("DateOfPay");
            DisposalDocument.Columns.Add("Amount");
            DisposalDocument.Columns.Add("payType");
            DisposalDocument.Columns.Add("ThisFor");
            DisposalDocument.Columns.Add("Notes");
            DisposalDocument.Columns.Add("Vat");
            DisposalDocument.Columns.Add("TotalAmount");
            DisposalDocument.Columns.Add("UserName");
            DisposalDocument.Columns.Add("AmountInArabic");
            DisposalDocument.Columns.Add("QrCode");
            var Doc = db.DisposalDocuments.Find(id);
            var userName = db.Users.Find(Doc.userId).Name;
            var utli = new Utlility.Utility();
         string qr = utli.GenrateQrCode(Doc.Id,Request.RawUrl);
                DataRow dataRow = DisposalDocument.NewRow();
                dataRow[0] = Doc.Id;
                dataRow[1] = Doc.DateOfPay;
                dataRow[2] = Doc.Amount;
                dataRow[3] = Doc.payType;
                dataRow[4] = Doc.Thisfor;
                dataRow[5] = Doc.Notes;
                dataRow[6] = Doc.Vat;
                dataRow[7] = Doc.TotalAmount;
                dataRow[8] = userName;
                dataRow[9] = N2C.ConvertN2C.ConvertNow(Doc.Amount, "ريال", "هلله");
                dataRow[10] = qr;
            DisposalDocument.Rows.Add(dataRow);
            DataSet MyDataSet = new DataSet("MyDataSet");
            MyDataSet.Tables.Add(DisposalDocument);
            rd.SetDataSource(MyDataSet);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string SavedFileName = string.Format("SalesDetails_{0}", DateTime.Now + ".pdf");

            return File(stream, "application/pdf", SavedFileName);
        }
        ///
        [AllowAnonymous]
        public ActionResult PrintDocument(int id)
        {
            string folderPath = "~/Doc/";
            string imagePath = "~/Doc/QrCode" + id+".jpg";
            var url = Request.Url.ToString();
            // If the directory doesn't exist then create it.
            if (!Directory.Exists(Server.MapPath(folderPath)))
            {
                Directory.CreateDirectory(Server.MapPath(folderPath));
            }
            var document = db.DisposalDocuments.Include(d => d.user).Where(d=>d.Id == id).ToList().FirstOrDefault();
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20);
            string barcodePath = Server.MapPath(imagePath);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    qrCodeImage.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                    
                    fs.Close();
                }
            }

          //  return View("Details", document);
            return new ActionAsPdf("Details" ,new {id = id});
        }

        // GET: DisposalDocuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisposalDocument disposalDocument = db.DisposalDocuments.Find(id);
            if (disposalDocument == null)
            {
                return HttpNotFound();
            }
            ViewBag.memberDataId = new SelectList(db.Users.Where(m=>m.Id == disposalDocument.userId), "Id", "Name", disposalDocument.userId);
            return View(disposalDocument);
        }

        // POST: DisposalDocuments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( DisposalDocument disposalDocument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(disposalDocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.memberDataId = new SelectList(db.Users.Where(m => m.Id == disposalDocument.userId), "Id", "Name", disposalDocument.userId);
            return View(disposalDocument);
        }

        // GET: DisposalDocuments/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisposalDocument disposalDocument = db.DisposalDocuments.Include(d=>d.user).Where(d=>d.Id == id).ToList().FirstOrDefault();
            if (disposalDocument == null)
            {
                return HttpNotFound();
            }
            return View(disposalDocument);
        }

        // POST: DisposalDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DisposalDocument disposalDocument = db.DisposalDocuments.Find(id);
            db.DisposalDocuments.Remove(disposalDocument);
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
