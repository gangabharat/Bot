using System.Net;
using System.Net.Mail;

namespace Bot.Services
{
    public interface IEmailService
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool EnableSsl { get; set; }
        public bool IsBodyHtml { get; set; }
        void Init(string? userName, string? password, string? host, int? port, bool? enableSsl, bool? isBodyHtml);
        Task SendMailAsync(string email, string subject, string htmlMessage);
        Task SendMailAsync(string email, string subject, string htmlMessage, string[] attachmentFiles);
    }
    public class EmailService : IEmailService
    {
        public string? Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool EnableSsl { get; set; } = true;
        public bool IsBodyHtml { get; set; } = true;
        private SmtpClient? SmtpClient { get; set; }
        public void Init(string? userName, string? password, string? host, int? port, bool? enableSsl, bool? isBodyHtml)
        {
            UserName = userName;
            Password = password;
            Host = host ?? Host;
            Port = port ?? Port;
            EnableSsl = enableSsl ?? EnableSsl;
            IsBodyHtml = isBodyHtml ?? IsBodyHtml;

            SmtpClient = new SmtpClient(Host)
            {
                Port = Port,
                Credentials = new NetworkCredential(UserName, Password),
                EnableSsl = EnableSsl
            };
        }
        public Task SendMailAsync(string email, string subject, string htmlMessage)
        {
            return SendMail(email, subject, htmlMessage, null);
        }
        public Task SendMailAsync(string email, string subject, string htmlMessage, string[] attachmentFiles)
        {
            return SendMail(email, subject, htmlMessage, attachmentFiles);
        }
        private Task SendMail(string email, string subject, string htmlMessage, string[]? attachmentFiles)
        {


            MailMessage mailMessage = new(UserName!, email, subject, htmlMessage)
            {
                IsBodyHtml = IsBodyHtml
            };

            if (attachmentFiles != null)
            {
                foreach (string file in attachmentFiles)
                {
                    Attachment attachment = new(file);
                    mailMessage.Attachments.Add(attachment);
                }
            }

            if (SmtpClient is null)
            {
                throw new ArgumentNullException($"use init method in {typeof(EmailService)}");
            }

            return SmtpClient.SendMailAsync(mailMessage);
        }
    }
}
