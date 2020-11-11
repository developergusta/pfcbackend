using System.Net;
using System.Net.Mail;
using Ticket2U.API.Models;

namespace Ticket2U.API.Services
{
    public class EmailService
    {

        public static void recoverPass(User user)
        {            
            string body = $"Sua nova senha é {user.Login.Pass}";
            string subject = "Recuperação de senha";
            enviaEmail(body, subject, user.Login.Email, user.Name);
        }

        public static void buyTicket(Ticket ticket)
        {            
            string body = $"Você comprou ingresso do evento {ticket.Event.TitleEvent}";
            string subject = $"Ingresso - {ticket.Event.TitleEvent}";
            enviaEmail(body, subject, ticket.User.Login.Email, ticket.User.Name);
        }

        public static void enviaEmail(string body, string subject, string emailUser, string nameUser)
        {
            var fromAddress = new MailAddress("reginaldokleber002@gmail.com", "Administrador - Ticket 2U");
            var emailObj = new MailAddress($"{emailUser}", $"{nameUser}");
            const string fromPassword = "pfcpass2020";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, emailObj)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

        }
    }
}