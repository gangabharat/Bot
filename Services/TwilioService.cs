using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Bot.Services
{
    public interface ITwilioService
    {
        string? TwilioPhoneNumber { get; set; }
        string? AccountSid { get; set; }
        string? AuthToken { get; set; }
        void Init(string? twilioPhoneNumber, string? accountSid, string? authToken);
        Task SendSMS(string phoneNumber, string textMessage);
    }
    public class TwilioService : ITwilioService
    {
        public void Init(string? twilioPhoneNumber, string? accountSid, string? authToken)
        {
            TwilioPhoneNumber = twilioPhoneNumber;
            AccountSid = accountSid;
            AuthToken = authToken;
        }
        public string? AccountSid { get; set; }
        public string? AuthToken { get; set; }
        public string? TwilioPhoneNumber { get; set; }

        public Task SendSMS(string phoneNumber, string textMessage)
        {
            TwilioClient.Init(AccountSid, AuthToken);

            var messageOptions = new CreateMessageOptions(new PhoneNumber(phoneNumber));
            messageOptions.From = new PhoneNumber(TwilioPhoneNumber);
            messageOptions.Body = textMessage;

            var message = MessageResource.Create(messageOptions);
            Console.WriteLine(message.Body);
            return Task.CompletedTask;
        }
    }
}