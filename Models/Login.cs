using System;
using System.ComponentModel.DataAnnotations;

namespace Ticket2U.API.Models
{
    public class Login
    {
        [Key]
        public int LoginId { get; set; }
        public String Email { get; set; }
        public String Pass { get; set; }
        public string Perfil { get; set; }

        public User User { get; set; }
    }
}