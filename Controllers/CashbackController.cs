using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket2U.API.Models;
using Ticket2U.API.Repositories;

namespace Ticket2U.API.Controllers
{
    [Route("[controller]")]
    public class CashbackController : Controller
    {
        private readonly EventRepository _repository;
        public CashbackController(EventRepository repository)
        {
            _repository = repository;
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> RequestCashback(Ticket ticket)
        {
            try
            {
                return this.StatusCode(StatusCodes.Status200OK, "Reembolso solicitado");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao recuperar senha: {ex}");
            }            
        }

        [Route("")]
        [HttpPut]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> ApproveCashback(Ticket ticket)
        {
            try
            {
                return this.StatusCode(StatusCodes.Status200OK, "Reembolso Aprovado");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao alterar status do cashback: {ex}");
            }            
        }
    }
}