using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Ticket2U.API.Models;

namespace Ticket2U.API.Services
{
    public class EmailService
    {

        public async Task recoverPass(User user)
        {            
            string body = $"Sua nova senha é {user.Login.Pass}";
            string subject = "Recuperação de senha";
            await enviaEmail(body, subject, user.Login.Email, user.Name);
        }

        public async Task buyTicket(Ticket ticket)
        {            
            string body = $"Você comprou ingresso do evento {ticket.Event.TitleEvent}";
            string subject = $"Ingresso - {ticket.Event.TitleEvent}";
            await enviaEmail(body, subject, ticket.User.Login.Email, ticket.User.Name);
        }

        public async Task eventApproved(User user, Event evento)
        {
            string body = $"Seu evento {evento.TitleEvent} foi aprovado por um administrador! Acesse a plataforma TICKET2U para conferir!";
            string subject = $"{evento.TitleEvent} - aprovado";
            await enviaEmail(body, subject, user.Login.Email, user.Name);
        }

        public async Task eventDenied(User user, Event evento)
        {
            string body = $"Seu evento {evento.TitleEvent} foi aprovado por um administrador! Acesse a plataforma TICKET2U para conferir!";
            string subject = $"{evento.TitleEvent} - aprovado";
            await enviaEmail(body, subject, user.Login.Email, user.Name);
        }

        public async Task enviaEmail(string body, string subject, string emailUser, string nameUser)
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