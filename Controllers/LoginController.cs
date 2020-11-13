
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Ticket2U.API.Models;
using Ticket2U.API.Repositories;
using Ticket2U.API.Services;

namespace Ticket2U.API.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserRepository _repository;

        public LoginController(UserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody]User userObj)
        {
            try
            {
                var user = await _repository.Login(userObj.Login.Email, Services.Encryptor.MD5Hash(userObj.Login.Pass));

                if (user == null)
                    return NotFound(new { message = "Usuário ou senha inválidos" });

                var token = TokenService.GenerateToken(user);
                user.Login.Pass = "";
                return new
                {
                    user = user,
                    token = token
                };
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }
    }
}