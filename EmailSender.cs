﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace E_Healthcare
{
    
        public class EmailSender : IEmailSender
        {
            private EmailSettings _emailSettings { get; }
            public EmailSender(IOptions<EmailSettings> emailSettings)
            {
                _emailSettings = emailSettings.Value;
            }
            public async Task Execute(string email, string subject, string message)
            {
                try
                {
                    string toEmail = string.IsNullOrEmpty(email) ?
                        _emailSettings.ToEmail : email;
                    MailMessage mailMessage = new MailMessage()
                    {
                        From = new MailAddress(_emailSettings.UsernameEmail, "My Email Display Name")
                    };
                    mailMessage.To.Add(toEmail);
                    mailMessage.CC.Add(_emailSettings.CcEmail);
                    mailMessage.Subject = "HealthCare:" + subject;
                    mailMessage.Body = message;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Priority = MailPriority.High;
                    using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                    {
                        smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mailMessage);
                    }

                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
            public Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                Execute(email, subject, htmlMessage).Wait();
                return Task.FromResult(0);
            }

        }
    }
