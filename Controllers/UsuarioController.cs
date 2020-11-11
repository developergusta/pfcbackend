using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket2U.API.Repositories;
using Ticket2U.API.Services;
using Ticket2U.API.Models;

namespace Ticket2U.API.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly UserRepository _repository;
        public UsuarioController(UserRepository repository)
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _repository.GetAllUsers();
                return Ok(users);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na listagem de usuários");
            }
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _repository.GetUserById(id);
                return Ok(user);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User userObj)
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

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                user.Status = "ATIVO";
                user.RegisterTime = DateTime.UtcNow;
                user.Login.Perfil = "USUARIO";
                user.Credit = 100;
                user.Login.Pass = Services.Encryptor.MD5Hash(user.Login.Pass);
                bool teste = await _repository.CreateUser(user);
                if (teste)
                {
                    return Ok(user);
                }
                else
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na criação do usuário");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro na criação: {ex}");
            }
        }

        [Route("Endereco/{id}")]
        [HttpGet]
        public async Task<Address> GetAddress(int id)
        {
            return await _repository.GetAddress(id);
        }

        [Route("Endereco")]
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> GetAllAdresses()
        {
            try
            {
                var addresses = await _repository.GetAllAdresses();
                return Ok(addresses);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na listagem de usuários");
            }
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _repository.GetUserById(id);

                if (user == null) return NotFound();
                else
                {
                    await _repository.RemoveUser(user);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar evento: {ex}");
            }
        }

        [Route("Address/{UserId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateAddressesUser(int UserId, [FromBody] User userObj)
        {
            try
            {
                var user = await _repository.GetUserById(UserId);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    return Created($"/Usuario/Address/{user.UserId}", userObj);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar usuário: {ex}");
            }
        }

        [Route("{UserId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(int UserId, [FromBody] User userObj)
        {
            try
            {
                var user = await _repository.GetUserById(UserId);

                if (user == null)
                {
                    return NotFound();
                }
                else
                {                    
                    await _repository.UpdateUser(userObj);                    

                    return Created($"/Usuario/{user.UserId}", userObj);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar usuário: {ex}");
            }
        }

        [Route("RecoverPass")]
        [HttpPost]
        public async Task<IActionResult> RecoverPass([FromBody] string cpf)
        {
            try
            {
                var user = await _repository.GetUserByCpf(cpf);

                if (user == null) return NotFound();
                else
                {
                    string newPass = RandomString(5);
                    await _repository.AlternPass(user.Login.Email, Services.Encryptor.MD5Hash(user.Login.Pass), Services.Encryptor.MD5Hash(newPass));
                    EmailService.recoverPass(user);
                    return this.StatusCode(StatusCodes.Status200OK, "Email enviado para recuperação de senha");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao recuperar senha: {ex.Message}");
            }
        }

        [Route("AlternPass")]
        [HttpPost]
        public async Task<IActionResult> AlternPass([FromBody] User[] Users)
        {
            try
            {
                var user = await _repository.GetUserByCpf(Users[0].Cpf);

                if (user == null)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"{Users[1].Login.Pass}, {Users[0].Login.Pass}, {Users[0].Cpf}");
                else
                {
                    user.Login = await _repository.AlternPass(user.Login.Email, Services.Encryptor.MD5Hash(Users[0].Login.Pass), Services.Encryptor.MD5Hash(Users[1].Login.Pass));
                    EmailService.recoverPass(user);
                    return this.StatusCode(StatusCodes.Status200OK, "Email enviado para recuperação de senha");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao recuperar senha: {ex}");
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [Route("BuyTicket")]
        [HttpPost]
        public async Task<IActionResult> BuyTicket([FromBody] Ticket ticket)
        {
            try
            {
                var user = await _repository.GetUserById(ticket.User.UserId);

                if (user == null) return NotFound();
                else
                {
                    await _repository.BuyTicket(ticket);
                    EmailService.recoverPass(user);
                    return this.StatusCode(StatusCodes.Status200OK, "Ingresso comprado com sucesso");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao comprar ingresso: {ex.Message}");
            }
        }
        
    }
}