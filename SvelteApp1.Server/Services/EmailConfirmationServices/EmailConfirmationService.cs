using MimeKit;
using MailKit;
using MailKit.Net.Smtp;

namespace SvelteApp1.Server.Services
{
    public class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly IConfiguration _configuration;
        public EmailConfirmationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendConfirmationEmailAsync(string toEmail, string confLink)
        {
            var emailMessage = new MimeMessage();
            Console.WriteLine(confLink);
            string htmlBody = $"Подтвердите регистрацию, перейдя по ссылке: <a href=\"{confLink}\" target=\"_top\">confirmation</a>";
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", _configuration[EmailSenderConfig.Email]));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = "Подтверждение почтового адреса";
            Console.WriteLine(htmlBody);
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlBody
            };
            using (var client = new SmtpClient())
            {
                
                await client.ConnectAsync("smtp.mail.ru", 465, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_configuration[EmailSenderConfig.Email], _configuration[EmailSenderConfig.Password]);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
