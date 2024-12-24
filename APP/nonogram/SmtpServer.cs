using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace nonogram
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
                var fromAddress = new MailAddress(_fromAddress, "Your Name");
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
                    IsBodyHtml = true // HTML formátumú e-mail küldése
                })
                {
                    await smtp.SendMailAsync(message);
                }

                Console.WriteLine("E-mail sikeresen elküldve!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt az e-mail küldésekor: {ex.Message}");
            }
        }
    }
}
