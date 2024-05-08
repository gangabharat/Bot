using Bot.AppSettings;
using Bot.Services;
using Microsoft.Extensions.Logging;

namespace Bot.Commands.Download
{   
    public class DownloadService 
    {
        private readonly ILogger<DownloadService> _logger;
        private readonly IEmailService email;

        public DownloadService(ILogger<DownloadService> logger ,IHttpClientService httpClient, IEmailService email) 
        {
            _logger = logger;
            HttpClient = httpClient;
            this.email = email;
        }

        public IHttpClientService HttpClient { get; }

        //public DownloadService(Logger<BaseService> log, IConfiguration configuration, 
        //    TwilioService twilioService, EmailService emailService, 
        //    HttpClientService httpClientService, FileSecureService fileSecureService) 
        //    : base(log, configuration, twilioService, emailService, 
        //          httpClientService, fileSecureService)
        //{
        //    //Log = log;
        //}

        //public ILogger< dLog { get; }

        //private ApplicationService AppServices { get; }

        //public DownloadService(ApplicationService applicationService)
        //{
        //    AppServices = applicationService;
        //}
        //public DownloadService(IConfiguration configuration, HttpClientService httpClient)
        //{
        //    HttpClient = httpClient;
        //    Options = configuration.GetRequiredSection(nameof(ApplicationOptions)).Get<ApplicationOptions>()!;
        //    //this.App = App;
        //}
        //public HttpClientService HttpClient { get; }
        //public ApplicationOptions Options { get; }
        ////public ApplicationService App { get; }

        public Task Download(List<string> lstUrls, int batchSize, string? path)
        {
            //Console.WriteLine("test");
            _logger.LogInformation("Info Log");
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(batchSize);
            var lstTask = lstUrls
                .Select((url, i) =>
                {
                    //string uniqueId = DateTime.Now.Ticks.ToString() + Random.Shared.Next(1000000, 999999999);
                    semaphoreSlim.Wait();

                    return Task.Run(() =>
                    {
                        try
                        {
                            var appl = new ApplicationOptions();
                            var res = HttpClient.Send(ApiType.GET, $"{url}","myToken", "").Result;

                            if (res.IsSuccessStatusCode)
                            {
                                //Services.Log.LogInformation($"downloaded : {url}");
                                //string uniqueId = DateTime.Now.Ticks.ToString() + Random.Shared.Next(1000000,999999999);
                                //Console.WriteLine($"{Guid.NewGuid()}");
                                using (var fs = new FileStream($"{Path.Combine(path!, $"{i}.jpeg")}", FileMode.Create))
                                {
                                    res.Content.CopyToAsync(fs).Wait();
                                }

                                //Services.FileSecure.Encrypt($"{Path.Combine(Services.Options.ApplicationOutputPath!,
                                //    $"{i}.txt")}", res.Content.ReadAsStringAsync().Result);

                                // Create a MemoryStream
                                //using (MemoryStream memoryStream = new MemoryStream())
                                //{
                                //    // Text to be written to the MemoryStream
                                //    string text = "Hello, this is some text.";

                                //    // Convert the text to bytes using UTF-8 encoding
                                //    byte[] textBytes = Encoding.UTF8.GetBytes(text);

                                //    // Write the bytes to the MemoryStream
                                //    memoryStream.Write(textBytes, 0, textBytes.Length);

                                //    // Optional: Reset the position of the MemoryStream to the beginning
                                //    memoryStream.Position = 0;

                                //    // Read the text back from the MemoryStream (optional)
                                //    byte[] buffer = new byte[memoryStream.Length];
                                //    memoryStream.Read(buffer, 0, buffer.Length);
                                //    string textFromMemoryStream = Encoding.UTF8.GetString(buffer);

                                //    // Output the text from the MemoryStream
                                //    Console.WriteLine("Text from MemoryStream: " + textFromMemoryStream);
                                //}
                            }
                            else
                            {
                                _logger.LogError($"download Failed : {url} - {res.StatusCode}");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error: {url} - {ex.InnerException}");
                        }
                        finally
                        {
                            semaphoreSlim.Release();
                        }
                    });
                });

            return Task.WhenAll(lstTask);
        }
    }
}
