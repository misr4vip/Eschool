using CrystalDecisions.CrystalReports.Engine;
using ESchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Data;
using QRCoder;

namespace ESchool.Controllers
{
   

    public class ReportController : Controller
    {
      private  ApplicationDbContext db = new ApplicationDbContext();
       
        //   Get
        public string GenrateQrCode(int id)
        {
            string folderPath = "~/Doc/";
            string imagePath = "~/Doc/QrCode.jpg";

            //  If the directory doesn't exist then create it.
            if (!Directory.Exists(HttpContext.Server.MapPath(folderPath)))
            {
                Directory.CreateDirectory(Server.MapPath(folderPath));
            }
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            //string path = Server.MapPath(Request.UserHostAddress+"/Doc/Document" + id+".pdf");
            string path = "www."+Request.UserHostAddress+"/Doc/Document" + id+".pdf";
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(path, QRCodeGenerator.ECCLevel.Q);
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

            return imagePath;
        }

        public ActionResult DisposalDocument(int id)
        {
            ReportDocument rd = new ReportDocument();          
            rd.Load(Path.Combine(Server.MapPath("~/Reports/DisposalDocumentRPT.rpt")));
            DataTable docDt = new DataTable("DisposalDocument");
            docDt.Columns.Add("Id");
            docDt.Columns.Add("DateOfPay");
            docDt.Columns.Add("Amount");
            docDt.Columns.Add("payType");
            docDt.Columns.Add("ThisFor");
            docDt.Columns.Add("Notes");
            docDt.Columns.Add("Vat");
            docDt.Columns.Add("TotalAmount");
            docDt.Columns.Add("userName");
            docDt.Columns.Add("AmountInArabic");
            docDt.Columns.Add("QrCode");
            var document = db.DisposalDocuments.Find(id);
            var user = db.Users.Find(document.userId);
            string imagePath = GenrateQrCode(id);
            string arabicAmount = N2C.ConvertN2C.ConvertNow(document.Amount, "ريال", "هلله");
            DataRow dataRow = docDt.NewRow();
            dataRow[0] = id;
            dataRow[1] = document.DateOfPay;
            dataRow[2] = document.Amount;
            dataRow[3] = document.payType;
            dataRow[4] = document.Thisfor;
            dataRow[5] = document.Notes;
            dataRow[6] = document.Vat;
            dataRow[7] = document.TotalAmount;
            dataRow[8] = user.Name;
            dataRow[9] = arabicAmount;
            dataRow[10] = imagePath;
            docDt.Rows.Add(dataRow);
            DataSet ds = new DataSet("MyDataSet");
            ds.DataSetName = "MyDataSet";
            ds.Tables.Add(docDt);
            rd.SetDataSource(ds);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string SavedFileName = string.Format("Document{0}", id + ".pdf");
            string path = Server.MapPath(string.Format("~/Doc/{0}",SavedFileName));
           //  var file = File(stream, "application/pdf", SavedFileName);
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
            //  return file;
            ViewBag.Id = id;
            return View();

        }

    }
}


