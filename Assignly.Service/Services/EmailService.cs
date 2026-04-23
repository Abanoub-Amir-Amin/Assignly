using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Assignly.Service.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly IConfiguration _configuration = configuration;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var client = new SmtpClient(
            _configuration["EmailCredintials:Host"],
            int.Parse(_configuration["EmailCredintials:Port"])
        )
        {
            Credentials = new NetworkCredential(
                _configuration["EmailCredintials:From"],
                _configuration["EmailCredintials:AppPassword"]
            ),
            EnableSsl = true,
        };
        var mail = new MailMessage
        {
            From = new MailAddress(
                _configuration["EmailCredintials:From"],
                _configuration["EmailCredintials:DisplayName"]
            ),
            IsBodyHtml = true,
            Subject = subject,
            Body = body,
        };
        mail.To.Add(to);
        await client.SendMailAsync(mail);
    }
}
