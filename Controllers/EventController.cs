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

namespace Ticket2U.API.Controllers
{
    [Route("[controller]")]
    public class EventController : Controller
    {
        private readonly EventRepository _repository;
        public EventController(EventRepository repository)
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            try
            {
                var events = await _repository.GetAllEvents();
                return Ok(events);
            }
            catch (Exception ex)
            {
                Console.Write($"Erro: {ex}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na listagem de eventos");
            }
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
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na listagem de eventos");
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
                    if(lot.DateEnd > now || lot.DateStart < now){
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

        
        [Route("EventsByUserId/{userId}")]
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

        [Route("NotApproved")]
        [HttpGet]
        public async Task<IActionResult> GetNotApprovedEvents()
        {
            try
            {
                var events = await _repository.GetNotApprovedEvents();

                return Ok(events);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao retornar eventos: {ex}");
            }
        }

        [Route("getByCategory/{category}")]
        [HttpGet]
        public async Task<IActionResult> GetEventByCategory( string category)
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


        [Route("")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event eventObj)
        {
            try
            {
                eventObj.Status = "PENDENTE";
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

        [Route("AdminCreate")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AdminCreateEvent(Event eventObj)
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
        public async Task<IActionResult> UpdateEvent(int EventId, Event eventObj)
        {
            try
            {
                var evento = await _repository.GetEvent(EventId);

                if (evento == null) return NotFound();
                else
                {
                    await _repository.UpdateEvent(evento);
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

        [Route("Endereco")]
        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            try
            {
                var results = await _repository.GetAllAddresses();
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
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

        [Route("Endereco")]
        [HttpPost]
        public string Create(Address address)
        {
            _repository.CreateAddress(address);
            return "Endereço salvo com sucesso";
        }

    }
}