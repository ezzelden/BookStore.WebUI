using BookStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;

namespace BookStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "azzdata49@gmail.com";
        public string MailFromAddress = "ezzeldensobhy@gmail.com";
        public bool UseSsl = true;
        public string Username = "ezzeldensobhy@gmail.com";
        public string Password = "24528179###";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"C:\ordres_bookstore_emails"; 
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSetting;
        public EmailOrderProcessor(EmailSettings setting)
        {
            emailSetting = setting;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSetting.UseSsl;
                smtpClient.Host = emailSetting.ServerName;
                smtpClient.Port = emailSetting.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSetting.Username,emailSetting.Password);
                if (emailSetting.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSetting.FileLocation;
                    smtpClient.EnableSsl = false;

                }
                StringBuilder body = new StringBuilder()
                    .AppendLine("A new Order has been Submitted")
                    .AppendLine("-------")
                    .AppendLine("Books:");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Book.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c})", line.Quantity, line.Book.Title, subtotal);
                }

                body.AppendFormat("Total Order Value:{0:c}", cart.ComputeTotalValue())
                    .AppendLine("-----")
                    .AppendLine("Ship To:")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2)
                    .AppendLine(shippingDetails.State)
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.Country)
                    .AppendLine("--------")
                    .AppendFormat("Gift Wrap:{0}", shippingDetails.GiftWrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(

                            emailSetting.MailFromAddress,
                            emailSetting.MailToAddress,
                            "New Order Submitted",
                            body.ToString()
                    );

                if (emailSetting.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {

                    Debug.Print(ex.Message);
                }
                
            }
        }
    }


}
