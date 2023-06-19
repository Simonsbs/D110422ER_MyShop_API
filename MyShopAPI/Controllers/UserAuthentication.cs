using System.ComponentModel.DataAnnotations;

namespace MyShopAPI.Controllers {
    public class UserAuthentication {
        [Required]
        public string UserName { get; set; }

        [Required] 
        public string Password { get; set; }
    }
}