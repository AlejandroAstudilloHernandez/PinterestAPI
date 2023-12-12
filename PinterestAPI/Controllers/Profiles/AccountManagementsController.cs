using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinterestAPI.Models;
using Microsoft.AspNetCore.Cors;

namespace PinterestAPI.Controllers.Profiles
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountManagementsController : ControllerBase
    {
        private readonly PinterestContext _context;

        public AccountManagementsController(PinterestContext context)
        {
            _context = context;
        }

        public class AccountManagementsControllerDto
        {
            public string Email { get; set; }
            public string Pass { get; set; }
            public DateTime Birthday { get; set; }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromForm] AccountManagementsControllerDto updateUserDto)
        {
            // Busca el usuario por Id
            var user = await _context.Users.FindAsync(userId);
            var profile = await _context.Profiles.FindAsync(userId);

            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Actualiza los campos solo si no son nulos en el DTO
            if (updateUserDto.Email != null)
            {
                user.Email = updateUserDto.Email;
                profile.Email = updateUserDto.Email;
            }

            if (updateUserDto.Pass != null)
            {                
                user.Pass = user.Pass = Encrypt.GetSHA256(updateUserDto.Pass.ToString());
            }

            if (updateUserDto.Birthday != DateTime.MinValue)
            {
                user.Birthday = updateUserDto.Birthday;
                profile.Birthday = updateUserDto.Birthday;
            }

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Usuario actualizado correctamente.");
        }

    }
}
