using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket2U.API.Repositories;
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
        public async Task<IActionResult> Create([FromBody]User user)
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
        

        [Route("{UserId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(int UserId,[FromBody] User userObj)
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

        [Route("Ban/{UserId}")]
        [Authorize(Roles = "Administrador")]
        [HttpPut]
        public async Task<IActionResult> Banir(int UserId,[FromBody] User userObj)
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

        [Route("UndoBan/{UserId}")]
        [Authorize(Roles = "Administrador")]
        [HttpPut]
        public async Task<IActionResult> Reativar(int UserId,[FromBody] User userObj)
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

        [Route("Address/Delete/{UserId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAddressUser(int UserId,[FromBody] Address addr)
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
                    await _repository.DeleteAddressUser(addr);
                    return Ok(addr);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar endereço: {ex.Message}");
            }
        }

        [Route("Phone/Delete/{UserId}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePhoneUser(int UserId,[FromBody] Phone phone)
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
                    await _repository.DeletePhoneUser(phone);
                    return Ok(phone);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar endereço: {ex.Message}");
            }
        }

    }
}