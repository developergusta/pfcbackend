using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket2U.API.Models;
using Ticket2U.API.Repositories;
using Ticket2U.API.Services;

namespace Ticket2U.API.Controllers
{
    [Route("[controller]")]
    public class TicketController : Controller
    {
        private readonly UserRepository _UserRepository;
        private readonly EventRepository _EventRepository;
        private readonly TicketRepository _TicketRepository;
        private readonly EmailService _email;

        public TicketController(UserRepository userRepository, EventRepository eventRepository, TicketRepository ticketRepository, EmailService email)
        {
            _UserRepository = userRepository;
            _EventRepository = eventRepository;
            _TicketRepository = ticketRepository;
            _email = email;
        }

        [Route("Buy")]
        [HttpPost]
        [Authorize(Roles = "USUARIO")]
        public async Task<IActionResult> BuyTicket([FromBody] List<Ticket> tickets)
        {
            try
            {
                int idUsuario = tickets[0].UserId.GetValueOrDefault();
                int idEvento = tickets[0].EventId.GetValueOrDefault();
                var user = await _UserRepository.GetUserById(idUsuario);
                if (user.Credit > 0)
                {
                    decimal valTotal = 0;
                    foreach (var item in tickets)
                    {
                        item.RegisterTime = DateTime.UtcNow.AddHours(-3);
                        int idCatg = item.LotCategoryId.GetValueOrDefault();
                        var lotcatg = await _EventRepository.GetLotCategoryById(idCatg);
                        valTotal = valTotal + lotcatg.PriceCategory;
                    }
                    if (valTotal > user.Credit)
                    {
                        return this.StatusCode(StatusCodes.Status500InternalServerError, "Saldo insuficiente");
                    }
                    var ticketsSoldByEvent = await _TicketRepository.GetTicketsSoldByEvent(idEvento);
                    var evento = await _EventRepository.GetEvent(idEvento);
                    if (ticketsSoldByEvent + tickets.Count > evento.Capacity)
                    {
                        await _TicketRepository.BuyTicket(tickets);
                        await _UserRepository.UpdateSaldo(valTotal, user);
                        await _email.buyTicket(tickets);
                        return Created($"/Ticket/{tickets[0].UserId}", tickets);
                    }
                    else
                    {
                        return this.StatusCode(StatusCodes.Status500InternalServerError, "Limite máximo de ingressos foi atingido");
                    }
                }
                else
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Saldo insuficiente");
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na compra de ingressos");
            }
        }

        [Route("User/{id}")]
        [HttpGet]
        [Authorize(Roles = "USUARIO,ADMINISTRADOR")]
        public async Task<IActionResult> GetTicketsByUserId(int id)
        {
            try
            {
                var user = await _UserRepository.GetUserById(id);
                if (user != null)
                {
                    var result = await _UserRepository.GetTicketsByUser(id);
                    return Ok(result);
                }
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Não foi possível localizar o usuário");
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na compra de ingressos");
            }
        }

        [Route("TicketsSoldByEvent/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetTicketsSoldByEvent(int id)
        {
            try
            {
                var result = await _TicketRepository.GetTicketsSoldByEvent(id);
                return Ok(result);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na compra de ingressos");
            }
        }

        [Route("Cashback")]
        [HttpGet]
        public async Task<IActionResult> GetCashbacks()
        {
            try
            {
                var result = await _TicketRepository.GetUsersTicketCashback();
                return Ok(result);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na compra listagem de cashbacks");
            }
        }

        [Route("Cashback")]
        [HttpPost]
        public async Task<IActionResult> RequestCashback([FromBody] Ticket ticketObj)
        {
            try
            {
                await _TicketRepository.RequestCashback(ticketObj);
                return this.StatusCode(StatusCodes.Status200OK, "Reembolso solicitado");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao recuperar senha: {ex}");
            }
        }

        #region ADMINSTRADOR
        [Route("Cashback/0")]
        [HttpPut]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> DenyCashback([FromBody] Cashback cashback)
        {
            try
            {
                User user = await _UserRepository.GetUserById(cashback.Ticket.User.UserId);
                await _TicketRepository.DenyCashback(cashback);                
                await _email.cashbackDenied(user, cashback);
                return this.StatusCode(StatusCodes.Status200OK, "Reembolso Negado");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao alterar status do cashback: {ex}");
            }
        }

        [Route("Cashback/1")]
        [HttpPut]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> ApproveCashback([FromBody]Cashback cashback)
        {
            try
            {
                User user = await _UserRepository.GetUserById(cashback.Ticket.User.UserId);
                await _TicketRepository.ApproveCashback(cashback);
                await _email.cashbackApproved(user, cashback);
                return this.StatusCode(StatusCodes.Status200OK, "Reembolso Aprovado");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao alterar status do cashback: {ex}");
            }
        }

        #endregion

    }
}