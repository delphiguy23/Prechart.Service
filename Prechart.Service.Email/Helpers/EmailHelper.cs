using MailKit.Security;
using MimeKit;
using Prechart.Service.Email.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Prechart.Service.Email.Helpers;

public class EmailHelper
{
    public static Task<EmailEventModel> SendEmail(EmailEventModel request, SmtpModel smtp)
    {
        var results = new EmailEventModel();

        if (request?.To is null || !request.To.Any())
        {
            results.Sent = false;
            results.Error = "No Email Recipients.";
            return Task.FromResult(results);
        }

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(smtp.Sender));

        if (request.To is not null && request.To.Any())
        {
            foreach (var t in request.To)
            {
                if (IsValidEmail(t))
                {
                    email.To.Add(MailboxAddress.Parse(t));
                }
            }
        }

        if (request.Bcc is not null && request.Bcc.Any())
        {
            foreach (var b in request.Bcc)
            {
                if (IsValidEmail(b))
                {
                    email.Bcc.Add(MailboxAddress.Parse(b));
                }
            }
        }

        if (request.Cc is not null && request.Cc.Any())
        {
            foreach (var c in request.Cc)
            {
                if (IsValidEmail(c))
                {
                    email.Cc.Add(MailboxAddress.Parse(c));
                }
            }
        }

        results.Sent = true;
        email.Subject = request.Subject;

        var builder = new BodyBuilder();

        if (request.Attachments is not null)
        {
            foreach (var file in request.Attachments)
            {
                builder.Attachments.Add(file.Filename, file.FileData);
            }
        }

        try
        {
            builder.HtmlBody = request.Body;
            email.Body = builder.ToMessageBody();
            using var smtpClient = new MailKit.Net.Smtp.SmtpClient();

            smtpClient.Connect(smtp.Host, Convert.ToInt32(smtp.Port), SecureSocketOptions.StartTls);
            smtpClient.Authenticate(smtp.Username, smtp.Password);
            smtpClient.Send(email);
            smtpClient.Disconnect(true);

        }
        catch (Exception ex)
        {
            results.Sent = false;
            results.Error = ex.ToString();
            return Task.FromResult(results);
        }

        return Task.FromResult(results);
    }
    private static bool IsValidEmail(string email)
    {
        var regex = new Regex(@"^[a-zA-Z0-9.a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9]+\.[a-zA-Z]+");
        var match = regex.Match(email);
        return match.Success;
    }
}