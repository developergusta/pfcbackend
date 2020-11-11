using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket2U.API.Models;
using Ticket2U.API.Repositories;

namespace Ticket2U.API.Controllers
{
    public class TicketController : Controller
    {
        private readonly UserRepository _UserRepository;
        private readonly EventRepository _EventRepository;
        public TicketController(UserRepository userRepository, EventRepository eventRepository)
        {
            _UserRepository = userRepository;
            _EventRepository = eventRepository;
        }

        [Route("Buy")]
        [HttpPost]
        [Authorize(Roles = "USUARIO")]
        public async Task<IActionResult> BuyTicket([FromBody]List<Ticket> tickets)
        {
            try
            {
                await _EventRepository.BuyTicket(tickets);
                return Created($"/Ticket/{tickets[0].UserId}", tickets);
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
                
                throw;
            }
        }
    }
}