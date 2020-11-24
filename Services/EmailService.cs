using System.Collections.Generic;
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
            enviaEmail(body, subject, user.Login.Email, user.Name);
        }

        public async Task buyTicket(List<Ticket> tickets)
        {            
            string body = $"Você comprou ingresso do evento {tickets[0].Event.TitleEvent}";
            string subject = $"Ingresso - {tickets[0].Event.TitleEvent}";
            enviaEmail(body, subject, tickets[0].User.Login.Email, tickets[0].User.Name);
        }

        public async Task eventApproved(User user, Event evento)
        {
            string body = $"Seu evento {evento.TitleEvent} foi aprovado por um administrador! Acesse a plataforma TICKET2U para conferir!";
            string subject = $"{evento.TitleEvent} - Aprovado";
            enviaEmail(body, subject, user.Login.Email, user.Name);
        }

        public async Task eventDenied(User user, Event evento)
        {
            string body = $"Seu evento {evento.TitleEvent} foi negado por um administrador! Caso queira entender melhor o motivo, entre em contato conosco!";
            string subject = $"{evento.TitleEvent} - Negado";
            enviaEmail(body, subject, user.Login.Email, user.Name);
        }

        public async Task cashbackApproved(User user, Cashback cashback)
        {
            string body = $"O cashback para seu ingresso do evento {cashback.Ticket.Event.TitleEvent} - {cashback.Ticket.LotCategory.Desc} no valor de R${cashback.Ticket.LotCategory.PriceCategory} foi aprovado por um administrador! Acesse a plataforma TICKET2U para conferir!";
            string subject = $"Cashback - Aprovado";
            enviaEmail(body, subject, user.Login.Email, user.Name);
        }

        public async Task cashbackDenied(User user, Cashback cashback)
        {
            string body = $"O cashback para seu ingresso do evento {cashback.Ticket.Event.TitleEvent} - {cashback.Ticket.LotCategory.Desc} no valor de R${cashback.Ticket.LotCategory.PriceCategory} foi negado por um administrador! Caso queira entender melhor o motivo, entre em contato conosco!";
            string subject = $"Cashback - Negado";
            enviaEmail(body, subject, user.Login.Email, user.Name);
        }

        private void enviaEmail(string body, string subject, string emailUser, string nameUser)
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