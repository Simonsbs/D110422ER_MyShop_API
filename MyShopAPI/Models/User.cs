using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MyShopAPI.Models
{
    public class User
    {
        public int ID { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}