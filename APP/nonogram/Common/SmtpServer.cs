using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace nonogram.Common
{
    public class SmtpServer
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _fromAddress;
        private readonly string _fromPassword;

        public SmtpServer(string smtpHost, int smtpPort, string fromAddress, string fromPassword)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _fromAddress = fromAddress;
            _fromPassword = fromPassword;
        }

        public async Task SendEmailAsync(string toAddress, string subject, string body)
        {
            try
            {
                var fromAddress = new MailAddress(_fromAddress, "NonoGram game");
                var toAddressObj = new MailAddress(toAddress, "Recipient Name");
                var smtp = new SmtpClient
                {
                    Host = _smtpHost,
                    Port = _smtpPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, _fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddressObj)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    await smtp.SendMailAsync(message);
                }
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
