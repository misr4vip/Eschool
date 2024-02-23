using System.IO;
using System.Web.Mvc;
using QRCoder;

namespace ESchool.Utlility
{
    public class Utility :Controller
    {
        private ESchool.Models.ApplicationDbContext db = new Models.ApplicationDbContext();
        // GET: Utitlity
        public string GenrateQrCode(int id, string Url)
        {
            string folderPath = "~/Doc/";
            string imagePath = "~/Doc/QrCode" + id + ".jpg";
            // If the directory doesn't exist then create it.
            if (!Directory.Exists(Server.MapPath(folderPath)))
            {
                Directory.CreateDirectory(Server.MapPath(folderPath));
            }
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(Url, QRCodeGenerator.ECCLevel.Q);
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

    }
}