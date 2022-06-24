using Google.Cloud.Functions.Framework;
using GrapeCity.Documents.Pdf;
using GrapeCity.Documents.Text;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace PDFExportCloudHttpFunction1
{
    public class Function : IHttpFunction
    {
        /// <summary>
        /// Logic for your function goes here.
        /// </summary>
        /// <param name="context">The HTTP context, containing the request and the response.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task HandleAsync(HttpContext context)
        {
            HttpRequest request = context.Request;
            string name = request.Query["name"].ToString();

            string Message = string.IsNullOrEmpty(name)
                ? "Hello, World!!"
                : $"Hello, {name}!!";

            //GcPdfDocument.SetLicenseKey("");

            GcPdfDocument doc = new GcPdfDocument();
            GcPdfGraphics g = doc.NewPage().Graphics;

            g.DrawString(Message,
                new TextFormat() { Font = StandardFonts.Helvetica, FontSize = 12 },
                new PointF(72, 72));

            byte[] output;

            using (var ms = new MemoryStream())
            {
                doc.Save(ms, false);
                output = ms.ToArray();
            }

            context.Response.ContentType = "application/pdf";
            context.Response.Headers.Add("Content-Disposition", "attachment;filename=Result.pdf");
            await context.Response.Body.WriteAsync(output);
        }
    }
}
