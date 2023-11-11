using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinterestAPI.Models;
using static PinterestAPI.Controllers.Profiles.ProfilesController;

namespace PinterestAPI.Controllers.Profiles
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivacyProfilesController : ControllerBase
    {
        private readonly PinterestContext _context;

        public PrivacyProfilesController(PinterestContext context)
        {
            _context = context;
        }

        [HttpPut("id")]
        public async Task<IActionResult> PutPrivacyProfileByUserId(int id, bool privacy)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null)
            {
                return BadRequest("El UsuarioId especificado no existe.");
            }


            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == usuario.UserId);

            if (profile != null)
            {
                // Actualiza la propiedad Privacy con el valor proporcionado en profileDto
                profile.Privacy = privacy;

                // Guarda los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok("La privacidad del perfil se ha actualizado correctamente.");
            }
            else
            {
                return NotFound("No se encontró un perfil para el UserId especificado.");
            }
        }


    }
}
