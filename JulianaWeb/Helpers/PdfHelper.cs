using SelectPdf;
using System.Drawing.Printing;
using System.Text;

namespace JulianaWeb.Helpers
{
  public class PdfHelper
  {

   


    
    public static byte[] ConvertCerlab(string title, string htmlCode, string footer, string contrasena="")
    {

      SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

      // header settings
      converter.Options.DisplayFooter = true;
      converter.Footer.DisplayOnFirstPage = true;
      converter.Footer.DisplayOnOddPages = true;
      converter.Footer.DisplayOnEvenPages = true;
      converter.Footer.Height = 50;

      

      //security
      if(contrasena != "")
      {
        converter.Options.SecurityOptions.UserPassword = contrasena;
      }

      //set document permissions
      converter.Options.SecurityOptions.CanAssembleDocument = false;
      converter.Options.SecurityOptions.CanCopyContent = false;
      converter.Options.SecurityOptions.CanEditAnnotations = false;
      converter.Options.SecurityOptions.CanEditContent = false;
      converter.Options.SecurityOptions.CanFillFormFields = true;
      converter.Options.SecurityOptions.CanPrint = true;

      // add some html content to the header
      PdfHtmlSection headerHtml = new PdfHtmlSection(footer, "/");
      headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
      converter.Footer.Add(headerHtml);

      converter.Options.PdfPageSize = PdfPageSize.A4;
      converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
      converter.Options.MarginLeft = 10;
      converter.Options.MarginRight = 10;
      converter.Options.MarginTop = 10;
      converter.Options.MarginBottom = 10;
      SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlCode, JW3.Helpers.MailHelper.FullyQualifiedApplicationPath);
      byte[] pdf = doc.Save();
      doc.Close();
      return pdf;
    }


    public static byte[] newconvert(string title, string htmlCode, string contrasena = "")
    {
 
      SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
      if (contrasena != "")
      {
        converter.Options.SecurityOptions.UserPassword = contrasena;
      }

      converter.Options.PdfPageSize = PdfPageSize.A4;
      converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
      converter.Options.MarginLeft = 20;
      converter.Options.MarginRight = 20;
      converter.Options.MarginTop = 20;
      converter.Options.MarginBottom = 20;
      SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlCode, JW3.Helpers.MailHelper.FullyQualifiedApplicationPath);
      byte[] pdf = doc.Save();
      doc.Close();
      return pdf;
    }
  }
}
