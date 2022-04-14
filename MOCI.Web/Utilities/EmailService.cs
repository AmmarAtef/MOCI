using MailKit.Net.Smtp;
using MimeKit;
using MOCI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MTRS.Web.Utilities
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IUserService _userService;

        const string _emailTemplatesPath = "/emailtemplates/";

        public EmailService(EmailSettings emailSettings, IUserService userService)
        {
            _emailSettings = emailSettings;
            _userService = userService;
        }
        public void SendEmail(string to, Dictionary<string, string> keys, string subject, string template)
        {
            List<string> emails = new List<string>();
            emails.Add(to);
            SendEmail(emails, keys, subject, template);
        }
        public void SendEmail(List<string> to, Dictionary<string, string> keys, string subject, string template)
        {

            var body = System.IO.File.ReadAllText(template);

            foreach (var item in keys)
            {
                body = body.Replace("##" + item.Key, item.Value);
                subject = subject.Replace("##" + item.Key, item.Value);
            }

            MailMessage message = new MailMessage();
            MailAddress from = new MailAddress(_emailSettings.From, "Admin");
            message.From = from;
            foreach (string item in to)
            {
                message.To.Add(item);
            }


           // message.To.Add("Mohamed.azayem@mannai.com.qa");

            message.Subject = subject;

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            message.Body = body;
            message.IsBodyHtml = true;

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(_emailSettings.Host);
            message.Subject = subject;


            client.Send(message);



        }
        public bool Send(long userId, long managerId, bool isApproved, string subject, string template)
        {
            try
            {
                var user = _userService.GetById(userId);
                var manager = _userService.GetById(managerId);

                var body = System.IO.File.ReadAllText(template);

                body = body.Replace("##UserName", user.FirstName).Replace("##ManagerName", manager.FirstName + " " + manager.LastName)
                    .Replace("##Approved", isApproved ? "Approved" : "Rejected");

                MailMessage message = new MailMessage();

                MailAddress from = new MailAddress(_emailSettings.From, "Admin");
                message.From = from;

                  MailAddress to = new MailAddress(user.Email, user.FirstName + " " + user.LastName);
              //  MailAddress to = new MailAddress("Mohamed.azayem@mannai.com.qa", user.FirstName + " " + user.LastName);
                message.To.Add(to);

                message.Subject = subject;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;
                message.Body = body;
                message.IsBodyHtml = true;

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(_emailSettings.Host);
                //  client.SslProtocols = System.Security.Authentication.SslProtocols.Tls11;

                // client.Connect(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.None);



                message.Subject = subject;


                client.Send(message);
               

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
