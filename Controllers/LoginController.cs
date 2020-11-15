
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Ticket2U.API.Models;
using Ticket2U.API.Repositories;
using Ticket2U.API.Services;
using System.Linq;

namespace Ticket2U.API.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly UserRepository _repository;
        private readonly EmailService _email;

        public LoginController(UserRepository repository, EmailService email)
        {
            _repository = repository;
            _email = email;
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

        
        [Route("RecoverPass")]
        [HttpPost]
        public async Task<IActionResult> RecoverPass(string cpf)
        {
            try
            {
                var user = await _repository.GetUserByCpf(cpf);

                if (user == null) return NotFound();
                else
                {
                    string newPass = RandomString(5);
                    await _repository.AlternPass(user.Login.Email, Services.Encryptor.MD5Hash(user.Login.Pass), Services.Encryptor.MD5Hash(newPass));
                    await _email.recoverPass(user);
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
        public async Task<IActionResult> AlternPass(User[] Users)
        {
            try
            {
                var user = await _repository.GetUserByCpf(Users[0].Cpf);

                if (user == null)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"{Users[1].Login.Pass}, {Users[0].Login.Pass}, {Users[0].Cpf}");
                else
                {
                    user.Login = await _repository.AlternPass(user.Login.Email, Services.Encryptor.MD5Hash(Users[0].Login.Pass), Services.Encryptor.MD5Hash(Users[1].Login.Pass));
                    await _email.recoverPass(user);
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
                
    }
}