using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.StyledXmlParser.Css.Media;
using System.Text;

namespace Bot.Commands.Html2Pdf
{
    public class Html2PdfService
    {
        private readonly ConverterProperties _converterProperties;
        public Html2PdfService()
        {
            _converterProperties = new ConverterProperties();
            _converterProperties.SetMediaDeviceDescription(new MediaDeviceDescription(MediaType.PRINT));
        }
        public Task ConvertToPdf(string sourceHtmlFileName, string destinationPdfFileName)
        {
            FileInfo htmlFileInfo = new(sourceHtmlFileName);
            FileInfo pdfFileInfo = new(destinationPdfFileName);

            HtmlConverter.ConvertToPdf(htmlFileInfo, pdfFileInfo, _converterProperties);
            //HtmlConverter.ConvertToPdf(htmlFileInfo, pdfFileInfo);
            return Task.CompletedTask;
        }
        public Task ConvertToPdfWithPasswordProtect(string sourceHtmlFileName, string destinationPdfFileName, string userPassword = "user", string ownerPassword = "owner")
        {
            var user = Encoding.UTF8.GetBytes($"{userPassword}");
            var owner = Encoding.UTF8.GetBytes($"{ownerPassword}");
            WriterProperties writerProperties = new WriterProperties();
            writerProperties.SetStandardEncryption(user, owner, EncryptionConstants.STANDARD_ENCRYPTION_40, EncryptionConstants.ENCRYPTION_AES_128);
            PdfWriter writer = new PdfWriter($"{destinationPdfFileName}", writerProperties);

            // Creating a PdfDocument       
            PdfDocument pdfDoc = new PdfDocument(writer);
            //File.
            HtmlConverter.ConvertToPdf(File.OpenRead(sourceHtmlFileName), pdfDoc);
            return Task.CompletedTask;
        }

        public Task ConvertToPdfWithPasswordProtect(Stream htmlStream, string destinationPdfFileName, string userPassword = "user", string ownerPassword = "owner")
        {
            var user = Encoding.UTF8.GetBytes($"{userPassword}");
            var owner = Encoding.UTF8.GetBytes($"{ownerPassword}");
            WriterProperties writerProperties = new WriterProperties();
            writerProperties.SetStandardEncryption(user, owner, EncryptionConstants.STANDARD_ENCRYPTION_40, EncryptionConstants.ENCRYPTION_AES_128);
            PdfWriter writer = new PdfWriter($"{destinationPdfFileName}", writerProperties);

            // Creating a PdfDocument       
            PdfDocument pdfDoc = new PdfDocument(writer);
            
            HtmlConverter.ConvertToPdf(htmlStream, pdfDoc);
            return Task.CompletedTask;
        }
    }
}
