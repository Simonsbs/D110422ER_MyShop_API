using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyShopAPI.Contexts;
using MyShopAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyShopAPI.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase {
        private readonly MyShopContext _context;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public AuthenticationController(IConfiguration config, ILogger<ProductsController> logger, MyShopContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        [HttpPost]
        public ActionResult Authenticate(UserAuthentication userAuthentication) {
            var user = AuthenticateUser(userAuthentication);
            if (user == null) {
                return Unauthorized();
            }

            var key = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"] ?? throw new ArgumentNullException("Authentication:SecretForKey not set"))
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> {
                new Claim("sub", user.ID.ToString()),
            };

            var token = new JwtSecurityToken(
                _config["Authentication:Issuer"],
                _config["Authentication:Audience"],
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(10),
                creds
                );

            var result = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(result);
        }

        private User AuthenticateUser(UserAuthentication auth) {
            return AuthenticateUser(auth.UserName, auth.Password);
        }
        private User AuthenticateUser(string userName, string password) {
            User authUser = _context.Users.FirstOrDefault(u => u.Username == userName && u.Password == password) ?? null!;
            return authUser;
        }
    }
}
