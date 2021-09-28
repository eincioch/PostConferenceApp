using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using CertificateCommon;

namespace CertificateImageGenerator
{
    public class DiplomaGenerator
    {


        public async Task<Byte[]> GetDiploma(DiplomaProperties certificateProperties, AttendeProperties studentProperties) {

            string name = studentProperties.FullName;
            int nameFontSize = 26;

            string description = certificateProperties.DescriptionLine1;
            string description2 = certificateProperties.DescriptionLine2;

            string webinarDate = certificateProperties.CourseDate;

            int descriptionFontSize = 14;
            var descriptionColor = new SolidBrush(Color.FromArgb(0x9C, 0x0D, 0x38));

            if (name.Length > 32)
                nameFontSize = 24;
            else if (name.Length > 48)
                nameFontSize = 22;

            Font nameDrawFont = new("Arial", nameFontSize, FontStyle.Bold);
            Font descriptionDrawFont = new("Arial", descriptionFontSize, FontStyle.Bold);


            StringFormat sf = new()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };


            var imageFromUri = await Helpers.ImageDownloaderHelper.DownloadPhoto(certificateProperties.DiplomaTemplateUrl);


            using var memoryStreamImage = new MemoryStream(imageFromUri);
            using Image image = Image.FromStream(memoryStreamImage); //or .jpg, etc...
            Graphics graphics = Graphics.FromImage(image);

            graphics.DrawString(name, nameDrawFont, Brushes.Black, new Rectangle(0, 180, 721, 120), sf);

            graphics.DrawString(description, descriptionDrawFont, descriptionColor, new Rectangle(0, 220, 721, 120), sf);
            graphics.DrawString(description2, descriptionDrawFont, descriptionColor, new Rectangle(0, 245, 721, 120), sf);


            graphics.DrawString(webinarDate, descriptionDrawFont, descriptionColor, new Rectangle(550, 460, 150, 120), sf);

            graphics.Save();
            using var ms = new MemoryStream();
            image.Save(ms, image.RawFormat);
            return ms.ToArray();
        }


    }
}
