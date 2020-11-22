using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket2U.API.Repositories;
using Ticket2U.API.Models;
using Ticket2U.API.Services;

namespace Ticket2U.API.Controllers
{
    [Route("[controller]")]
    public class EventController : Controller
    {
        private readonly EventRepository _repository;
        private readonly UserRepository _userRepository;
        private readonly EmailService _email;
        public EventController(EventRepository repository, UserRepository userRepository, EmailService email)
        {
            _repository = repository;
            _userRepository = userRepository;
            _email = email;
        }


        [Route("")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] Event eventObj)
        {
            try
            {
                eventObj.Status = "PENDENTE";
                if (eventObj.Lots == null)
                {
                    var lot = new Lot();
                    lot.DateStart = eventObj.DateStart;
                    lot.DateEnd = eventObj.DateEnd;
                    eventObj.Lots.Add(lot);                    
                    var lotCatg = new LotCategory();
                    eventObj.Lots[0].LotCategories.Add(lotCatg);
                }
                bool eventResult = await _repository.CreateEvent(eventObj);
                if (eventResult)
                {
                    return Created($"/Event/{eventObj.EventId}", eventObj);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }

            return BadRequest();
        }

        [Route("Today")]
        [HttpGet]
        public async Task<IActionResult> GetEventsToday()
        {
            try
            {
                var events = await _repository.GetEventsToday();
                return Ok(events);
            }
            catch (Exception ex)
            {
                Console.Write($"Erro: {ex}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na listagem de eventos");
            }
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetEvent(int id)
        {
            try
            {
                var events = await _repository.GetEvent(id);
                return Ok(events);
            }
            catch (Exception ex)
            {
                Console.Write($"Erro: {ex}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na listagem de evento");
            }
        }

        [Route("LotByDate/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetLotByDate(int id)
        {
            try
            {
                var evento = await _repository.GetEvent(id);
                DateTime now = DateTime.UtcNow;
                var i = 0;
                foreach (var lot in evento.Lots)
                {
                    if (lot.DateEnd > now || lot.DateStart < now)
                    {
                        evento.Lots.RemoveAt(i);
                    }
                    i++;
                }
                return Ok(evento);
            }
            catch (Exception ex)
            {
                Console.Write($"Erro: {ex}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na busca de lote");
            }
        }

        [Route("LotCategoryByTicket/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetLotCategoryByTicket(int id)
        {
            try
            {
                var lotCateg = await _repository.GetLotCategoryById(id);
                return Ok(lotCateg);
            }
            catch (Exception ex)
            {
                Console.Write($"Erro: {ex}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na busca de categoria de lote");
            }
        }

        [Route("EventsByUserId/{userId}")]
        [Authorize(Roles = "ADMINISTRADOR,USUARIO")]
        [HttpGet]
        public async Task<IActionResult> GetEventsByUserId(int userId)
        {
            try
            {
                var events = await _repository.GetEventsByUserId(userId);

                return Ok(events);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao retornar eventos: {ex}");
            }
        }

        [Route("EventsMostSold")]
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet]
        public async Task<IActionResult> GetMostSoldEventsOnYear()
        {
            try
            {
                var events = await _repository.GetMostSoldEventsOnYear();

                return Ok(events);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao retornar eventos: {ex}");
            }
        }

        [Route("Approved")]
        [HttpGet]
        public async Task<IActionResult> GetApprovedEvents()
        {
            try
            {
                var events = await _repository.GetApprovedEvents();

                return Ok(events);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao retornar eventos: {ex}");
            }
        }

        [Route("Pending")]
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet]
        public async Task<IActionResult> GetNotApprovedEvents()
        {
            try
            {
                var events = await _repository.GetPendingEvents();

                return Ok(events);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao retornar eventos: {ex}");
            }
        }

        [Route("Denied")]
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet]
        public async Task<IActionResult> GetDenidEvents()
        {
            try
            {
                var events = await _repository.GetDeniedEvents();

                return Ok(events);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao retornar eventos: {ex}");
            }
        }

        [Route("getByCategory/{category}")]
        [HttpGet]
        public async Task<IActionResult> GetEventByCategory(string category)
        {
            try
            {
                var events = await _repository.GetEventByCategory(category);

                return Ok(events);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao retornar eventos: {ex}");
            }
        }

        [Route("AdminCreate")]
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPost]
        public async Task<IActionResult> AdminCreateEvent([FromBody]Event eventObj)
        {
            try
            {
                eventObj.Status = "APROVADO";
                bool eventResult = await _repository.CreateEvent(eventObj);
                if (eventResult)
                {
                    return Created($"/Event/{eventObj.EventId}", eventObj);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }

            return BadRequest();
        }

        [Route("{EventId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateEvent(int EventId, [FromBody] Event eventObj)
        {
            try
            {
                var evento = await _repository.GetEvent(EventId);

                if (evento == null) return NotFound();
                else
                {
                    await _repository.UpdateEvent(eventObj);
                    return Created($"/Event/{eventObj.EventId}", evento);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar evento: {ex}");
            }
        }

        [Route("Approve/{EventId}")]
        [HttpPut]
        public async Task<IActionResult> ApproveEvent(int EventId, [FromBody]Event eventObj)
        {
            try
            {
                var evento = await _repository.GetEvent(EventId);

                if (evento == null) return NotFound();
                else
                {
                    var idUser = evento.UserId.GetValueOrDefault();
                    await _repository.ApproveEvent(evento);
                    var user = await _userRepository.GetUserById(idUser);
                    await _email.eventApproved(user, evento);
                    return Created($"/Event/{eventObj.EventId}", evento);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar evento: {ex}");
            }
        }

        [Route("Deny/{EventId}")]
        [HttpPut]
        public async Task<IActionResult> DenyEvent(int EventId, [FromBody] Event eventObj)
        {
            try
            {
                var evento = await _repository.GetEvent(EventId);

                if (evento == null) return NotFound();
                else
                {
                    var idUser = evento.UserId.GetValueOrDefault();
                    await _repository.DenyEvent(evento);
                    var user = await _userRepository.GetUserById(idUser);
                    await _email.eventDenied(user, evento);
                    return Created($"/Event/{eventObj.EventId}", evento);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar evento: {ex}");
            }
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var evento = await _repository.GetEvent(id);

                if (evento == null) return NotFound();
                else
                {
                    await _repository.RemoveEvent(evento);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar evento: {ex}");
            }
        }

        [Route("Address/Delete/{LotId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteLotEvent(int LotId)
        {
            try
            {
                var lot = await _repository.GetLotById(LotId);
                if (lot == null)
                {
                    return NotFound();
                }
                else
                {
                    await _repository.DeleteLot(lot);
                    return Ok(lot);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar lote: {ex.Message}");
            }
        }

        [Route("LotCategory/Delete/{LotCategoryId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteLotCategoryEvent(int LotCategoryId)
        {
            try
            {
                var lotCateg = await _repository.GetLotCategoryById(LotCategoryId);
                if (lotCateg == null)
                {
                    return NotFound();
                }
                else
                {
                    await _repository.DeleteLotCategory(lotCateg);
                    return Ok(lotCateg);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar categoria de lote: {ex.Message}");
            }
        }

        [Route("Endereco/{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var results = await _repository.GetAddress(id);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            };
        }
    }
}