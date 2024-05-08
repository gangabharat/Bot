using Bot.Common;
using Microsoft.Extensions.Configuration;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Bot.Commands.Html2Pdf
{
    public class Html2PdfCommand : Command<Html2PdfSettings>
    {
        //private readonly IBaseService _baseService;
        private readonly Html2PdfService _html2PdfService;

        public Html2PdfCommand(Html2PdfService html2PdfService)
        {
            //_baseService = baseService;
            _html2PdfService = html2PdfService;
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Html2PdfSettings settings)
        {

            //settings.HtmlSourcePath
            //var res = _baseService.HttpClient.Send(ApiType.GET, "https://jsonplaceholder.typicode.com/photos").Result;

            //var fileName = Path.Combine(_baseService.Options.ApplicationOutputPath!, $"{Guid.NewGuid()}.pdf");

            //_html2PdfService.ConvertToPdfWithPasswordProtect(res.Content.ReadAsStream(), fileName).GetAwaiter().GetResult();


            // Create a MemoryStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Text to be written to the MemoryStream
                string text = "Hello, this is some text.";

                // Convert the text to bytes using UTF-8 encoding
                byte[] textBytes = Encoding.UTF8.GetBytes(text);

                // Write the bytes to the MemoryStream
                memoryStream.Write(textBytes, 0, textBytes.Length);

                // Optional: Reset the position of the MemoryStream to the beginning
                memoryStream.Position = 0;

                // Read the text back from the MemoryStream (optional)
                byte[] buffer = new byte[memoryStream.Length];
                memoryStream.Read(buffer, 0, buffer.Length);
                string textFromMemoryStream = Encoding.UTF8.GetString(buffer);

                // Output the text from the MemoryStream
                Console.WriteLine("Text from MemoryStream: " + textFromMemoryStream);
            }

            //string[] attachments = [fileName];
            //var  ..fileName
            //_baseService.Email.SendMailAsync("devgangabharat@gmail.com", "Json Subject", res.Content.ReadAsStringAsync().Result, [fileName]).Wait();
            //pdf.GetAwaiter().GetResult();

            //Path.GetTempFileName()
            //var table = new Table();

            //table.AddColumn("Date");
            //table.AddColumn("Temp");
            //table.AddColumn("Summary");

            //foreach (var forecast in forecasts)
            //{
            //    var temperature = unit == TemperatureUnit.Fahrenheit
            //        ? forecast.TemperatureF
            //        : forecast.TemperatureC;

            //    table.AddRow(
            //        forecast.Date.ToShortDateString(),
            //        temperature.ToString(),
            //        forecast.Summary);
            //}

            //table.Expand();

            //AnsiConsole.Write(table);

            return 0;
        }
    }
}
