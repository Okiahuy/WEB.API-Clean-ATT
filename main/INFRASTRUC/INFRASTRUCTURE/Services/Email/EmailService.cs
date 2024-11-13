using APPLICATIONCORE.Interface.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace INFRASTRUCTURE.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly string _emailSender;
        private readonly string _emailPassword;

        public EmailService(string emailSender, string emailPassword)
        {
            _emailSender = emailSender;
            _emailPassword = emailPassword;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("HOA TƯƠI CỤC CƯNG CỦA KEM", _emailSender));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = body };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_emailSender, _emailPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc xử lý theo cách bạn muốn
                Console.WriteLine($"Đã xảy ra lỗi khi gửi email: {ex.Message}");
            }
        }
    }
}
