using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket2U.API.Models;
using Ticket2U.API.Repositories;

namespace Ticket2U.API.Controllers
{
    [Route("[controller]")]
    public class TicketController : Controller
    {
        private readonly UserRepository _UserRepository;
        private readonly EventRepository _EventRepository;
        private readonly TicketRepository _TicketRepository;
        public TicketController(UserRepository userRepository, EventRepository eventRepository, TicketRepository ticketRepository)
        {
            _UserRepository = userRepository;
            _EventRepository = eventRepository;
            _TicketRepository = ticketRepository;
        }

        [Route("Buy")]
        [HttpPost]
        [Authorize(Roles = "USUARIO")]
        public async Task<IActionResult> BuyTicket([FromBody] List<Ticket> tickets)
        {
            try
            {
                int idUsuario = tickets[0].UserId.GetValueOrDefault();
                var user = await _UserRepository.GetUserById(idUsuario);
                if (user.Credit > 0)
                {
                    decimal valTotal = 0;
                    foreach (var item in tickets)
                    {
                        int idCatg = item.LotCategoryId.GetValueOrDefault();
                        var lotcatg = await _EventRepository.GetLotCategoryById(idCatg);
                        valTotal = valTotal + lotcatg.PriceCategory;
                    }
                    if (user.Credit > valTotal ){
                        return this.StatusCode(StatusCodes.Status500InternalServerError, "Saldo insuficiente");
                    }
                    await _TicketRepository.BuyTicket(tickets);
                    await _UserRepository.UpdateSaldo(valTotal, user);
                    return Created($"/Ticket/{tickets[0].UserId}", tickets);
                }
                else{
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
                var result = await _UserRepository.GetTicketsByUser(user);
                return Ok(result);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na compra de ingressos");
            }
        }
    }
}