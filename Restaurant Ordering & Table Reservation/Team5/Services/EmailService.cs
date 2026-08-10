using MailKit.Net.Smtp;
using MimeKit;

namespace Team5.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string toEmail, string toName, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                _configuration["EmailSettings:SenderName"],
                _configuration["EmailSettings:SenderEmail"]
            ));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            client.Connect(
                _configuration["EmailSettings:SmtpServer"],
                int.Parse(_configuration["EmailSettings:Port"]),
                MailKit.Security.SecureSocketOptions.StartTls
            );
            client.Authenticate(
                _configuration["EmailSettings:SenderEmail"],
                _configuration["EmailSettings:AppPassword"]
            );
            client.Send(message);
            client.Disconnect(true);
        }
    }
}
