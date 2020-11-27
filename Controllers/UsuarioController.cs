using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket2U.API.Repositories;
using Ticket2U.API.Models;
using System.IO;
using Ticket2U.API.Services;

namespace Ticket2U.API.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly UserRepository _repository;
        private readonly EmailService _email;
        public UsuarioController(UserRepository repository, EmailService email)
        {
            _repository = repository;
            _email = email;
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

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                user.Status = "ATIVO";
                user.RegisterTime = DateTime.UtcNow.AddHours(-3);
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
        [Authorize(Roles = "Administrador")]
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


        [Route("Address/Delete/{AddressId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAddressUser(int AddressId)
        {
            try
            {
                await _repository.DeleteAddressUser(AddressId);
                return Ok("Endereço excluído com sucesso");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar endereço: {ex.Message}");
            }
        }

        [Route("Phone/Delete/{PhoneId}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePhoneUser(int PhoneId)
        {
            try
            {
                await _repository.DeletePhoneUser(PhoneId);
                return Ok("Telefone Excluído com Sucesso");

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar endereço: {ex.Message}");
            }
        }

        #region ADMINISTRADOR

        [Route("")]
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> GetUsersList()
        {
            try
            {
                var users = await _repository.GetUsersList();
                return Ok(users);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro na listagem de usuários");
            }
        }

        [Route("Ban/{UserId}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet]
        public async Task<IActionResult> BanUser(int UserId)
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
                    await _repository.BanUser(user);

                    return Created($"/Usuario/{user.UserId}", user);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar usuário: {ex}");
            }
        }

        [Route("Reactivate/{UserId}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet]
        public async Task<IActionResult> ReactivateUser(int UserId)
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
                    await _repository.ReactivateUser(user);

                    return Created($"/Usuario/{user.UserId}", user);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar usuário: {ex}");
            }
        }

        #endregion

        [Route("Email")]
        [HttpGet]
        public async Task<IActionResult> SendMail(int UserId)
        {
            try
            {
                await _email.TesteMail();
                return Ok("email enviado");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar usuário: {ex}");
            }
        }
    }
}