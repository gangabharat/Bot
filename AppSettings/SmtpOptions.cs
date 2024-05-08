namespace Bot.AppSettings
{
    public class SmtpOptions
    {
        public string? SmtpClient { get; set; }
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool EnableSsl { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}
