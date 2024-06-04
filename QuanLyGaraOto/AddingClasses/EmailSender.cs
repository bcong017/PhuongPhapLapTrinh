using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGaraOto.AddingClasses
{
    public class EmailSender
    {
        private static EmailSender instance;
        public static EmailSender Instance
        {
            get
            {
                if (instance == null)
                    instance = new EmailSender();
                return instance;
            }
            set { instance = value; }
        }

        private int verifiedNumber;
        public int VerifiedNumber
        {
            get { return verifiedNumber; }
            set { verifiedNumber = value; }
        }

        private string gmail;
        public string Gmail
        {
            get { return gmail; }
            set { gmail = value; }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private EmailSender()
        { }

        async public void SendEmail(string verifiedEmail, string verifiedPassword)
        {
            VerifiedNumber = GenerateRandomNumber();
            var fromAddress = new MailAddress(verifiedEmail, "Quản lý Gara Oto");
            var toAddress = new MailAddress(Gmail);

            string subject = "Email tự động";
            string body = "Mã xác thực của bạn là: " + verifiedNumber;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, verifiedPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }
        }

        int GenerateRandomNumber()
        {
            var random = new Random();
            return random.Next(0, 9999);
        }
    }
}
