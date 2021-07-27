using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using HandlebarsDotNet;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EmailTest.Classes
{
    public class EmailService
    {
        private const int port = 25;
        private const string smtpAddress = "SMTP-relay.au.fcl.internal";
        private const string testEmailTemplate = @".\EmailTemplates\TestEmail.html";
        private const string fromEmailAddress = "david.arrebola@flightcentre.com";

        public void CreateAndSendTripApprovalEmail(TravelRequest travelRequest, EmailType type)
        {
            var template = PrepareTemplateEmailForSMTP(travelRequest, type);
            var message = new MimeMessage();

            var from = new MailboxAddress("Admin", fromEmailAddress);
            message.From.Add(from);

            var to = new MailboxAddress("User", travelRequest.ApproverEmail);
            message.To.Add(to);

            message.Subject = "Approval Email";

            var builder = new BodyBuilder {HtmlBody = template};

            message.Body = builder.ToMessageBody();

            SendEmail(message);
        }
        private static void SendEmail(MimeMessage message)
        {
            try
            {
                var client = new SmtpClient();
                client.Connect(smtpAddress, port, false);
                //client.Authenticate("user_name_here", "pwd_here"); // In case we need Auth when live.

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private static string PrepareTemplateEmailForSMTP(TravelRequest travelRequest, EmailType type)
        {
            try
            {
                // get template
                var template = string.Empty;
                if (type == EmailType.TripApproval)
                {
                    template = File.ReadAllText(testEmailTemplate);
                }

                // populate template with data
                var compiledTemplate = Handlebars.Compile(template);
                var populatedTemplate = compiledTemplate(travelRequest);

                return populatedTemplate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
