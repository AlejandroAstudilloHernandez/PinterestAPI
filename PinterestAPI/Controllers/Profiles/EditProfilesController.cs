using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinterestAPI.Models;
using Microsoft.AspNetCore.Cors;
using static PinterestAPI.Controllers.Pins.CreatesPinsController;

namespace PinterestAPI.Controllers.Profiles
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EditProfilesController : ControllerBase
    {
        private readonly PinterestContext _context;

        public EditProfilesController (PinterestContext context)
        {
            _context = context;
        }

        public class EditProfileDto
        {
            public IFormFile ProfilePhoto { get; set; } = null!;
            public string Name { get; set; }
            public string Lastname { get; set; }
            public string Information { get; set; }
            public string WebSite { get; set; }
            public string Username { get; set;}
        }

        //Metodo para convertir la imagen
        private async Task<byte[]> ObtenerBytesImagen(IFormFile archivoImagen)
        {
            using var ms = new MemoryStream();
            await archivoImagen.CopyToAsync(ms);
            return ms.ToArray();
        }


        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateProfile(int userId, [FromForm] EditProfileDto editProfileDto)
        {
            // Busca el usuario por Id
            var user = await _context.Profiles.FindAsync(userId);

            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Actualiza los campos solo si no son nulos en el DTO
            if (editProfileDto.ProfilePhoto != null)
            {
                var imageBytes = await ObtenerBytesImagen(editProfileDto.ProfilePhoto);
                user.ProfilePhoto = imageBytes;            
            }

            if (editProfileDto.Name != null)
            {
                user.Name = editProfileDto.Name;
            }

            if (editProfileDto.Lastname != null)
            {
                user.Lastname = editProfileDto.Lastname;
            }

            if (editProfileDto.Information != null)
            {
                user.Information = editProfileDto.Information;
            }

            if (editProfileDto.WebSite != null)
            {
                user.WebSite = editProfileDto.WebSite;
            }

            if (editProfileDto.Username != null)
            {
                user.UserName = editProfileDto.Username;
            }

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Perfil actualizado correctamente.");
        }

    }
}
