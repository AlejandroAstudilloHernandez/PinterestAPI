using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PinterestAPI.Models;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PinterestAPI.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly PinterestContext _context;
        private readonly IConfiguration _config;


        public LoginsController(PinterestContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public class LoginCredentials
        {
            public string Email { get; set; }
            public string Pass { get; set; }
        }

        // POST: api/Login
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginCredentials loginCredentials)
        {
            // Busca el usuario en la base de datos por correo electrónico
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginCredentials.Email);

            if (user == null)
            {
                return BadRequest("Usuario inexistente.");
            }

            // Si el usuario no existe o la contraseña es incorrecta, devuelve un error
            if (!UserExists(user.Email) || user.Pass != Encrypt.GetSHA256(loginCredentials.Pass.ToString()))
            {
                return Unauthorized("Credenciales incorrectas");
            }

            // Genera un token de autenticación para el usuario (puedes utilizar JWT u otro método)
            var jwtToken = GenerateToken(user);

            // Devuelve el token como respuesta
            return Ok(new { Token = jwtToken});
        }



        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                                claims: claims,
                                expires: DateTime.Now.AddMinutes(60),
                                signingCredentials: creds
                );
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }

        private bool UserExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }
    }
}
