using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace ECommerce.API.Utilities
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config["EmailSettings:From"]));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:SmtpPort"]), false);
                await smtp.AuthenticateAsync(_config["EmailSettings:SmtpUser"], _config["EmailSettings:SmtpPass"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                Console.WriteLine($"Email sent: {to}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending error: {ex.Message}\n{ex.StackTrace}");
                throw; // Hata üst katmana da iletilsin
            }
        }

        public async Task SendVerificationEmailAsync(string to, string token)
        {
            string verifyUrl = $"http://localhost:5173/verify-email?token={token}";
            string body = $"To verify your account <a href='{verifyUrl}'>Click here</a>.<br/>Or paste this link into your browser: {verifyUrl}";
            await SendEmailAsync(to, "Email Verification", body);
        }
    }
} 