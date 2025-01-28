using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model.DTO.EmailDtosFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.DataAccess.RepositoryPattern
{
    public class EmailServices : IEmailServices
    {

        private readonly EmailConfiguration _config;

        public EmailServices(EmailConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(Message message, string scenario)
        {
            var emailMessage = CreateEmailMessage(message, scenario);

            await SendAsync(emailMessage);
        }
        private MimeMessage CreateEmailMessage(Message message, string scenario)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_config.From));
            emailMessage.To.Add(MailboxAddress.Parse(message.To.ToString()));
            emailMessage.Subject = message.Subject;

            string bodyContent = GetBodyContent(message.Content, scenario);

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = bodyContent
            };
            return emailMessage;
        }
        private string GetBodyContent(string content, string scenario)
        {
            switch (scenario)
            {
               
                case "SignupOTP":
                    return $"<h2 style='color:green'>Signup OTP</h2><br/><p>Your Parking Platform verification code is: <strong>{content}</strong></p><br/><p>Please use this code to complete your signup.</p>";
                case "PasswordResetOTP":
                    return $"<h2 style='color:green'>Password Reset OTP</h2><br/><p>Your reset OTP is: <strong>{content}</strong></p><br/><p>If you didn’t request this, please ignore this email.</p> ";
                case "ParkingSlotCofirm":
                    return $"<h2 style='color:green'>Parking Slot Confirm</h2><br/><p>{content}</p><br/><p>Stay safe and take care!</p>";
                case "CancelParkingSlot":
                    return $"<h2 style='color:red'>Parking Slot Cancel</h2><br/><p>Dear User,</p><br/><p>{content}</p><br/> <p>Thank you for visit Parking Platform</p>";
                default:
                    return $"<p>{content}</p>";
            }
        }
        private async Task SendAsync(MimeMessage mimeMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {

                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_config.Username, _config.Password);
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
                catch
                {
                    throw;
                }
            }

        }
    }
}
