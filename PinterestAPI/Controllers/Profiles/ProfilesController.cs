using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinterestAPI.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace PinterestAPI.Controllers.Profiles
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfilesController : ControllerBase
    {
        private readonly PinterestContext _context;

        public ProfilesController (PinterestContext context)
        {
            _context = context;
        }

        public class ProfileDto
        {
            public int ProfileId { get; set; }
            public string Name { get; set; }
            public string Lastname { get; set; }
            public string Information { get; set; }
            public string WebSite { get; set; }
            public string UserName { get; set; }
            public int UserId { get; set; }
            public DateTime? Birthday { get; set; }
            public string Email { get; set; }
            public byte[] ProfilePhoto { get; set; }
            public bool? Privacy { get; set; }
        }

        public class MiniProfileDto
        {            
            public string UserName { get; set; }
            public string Email { get; set; }
            public byte[] ProfilePhoto { get; set; }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfileByUserId(int id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null)
            {
                return BadRequest("El UsuarioId especificado no existe.");
            }

            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == usuario.UserId);

            if (profile != null)
            {
                var profileDto = new ProfileDto
                {
                    ProfileId = profile.ProfileId,
                    Name = profile.Name,
                    Lastname = profile.Lastname,
                    Information = profile.Information,
                    WebSite = profile.WebSite,
                    UserName = profile.UserName,
                    Birthday = profile.Birthday,
                    Email = profile.Email,
                    ProfilePhoto = profile.ProfilePhoto,
                    Privacy = profile.Privacy,
                };

                return Ok(profileDto);
            }
            else
            {
                return NotFound("No se encontró un perfil para el UsuarioId especificado.");
            }
        }



        [HttpGet("miniProfile/{id}")]
        public async Task<IActionResult> GetMiniProfileByUserId(int id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null)
            {
                return BadRequest("El UsuarioId especificado no existe.");
            }

            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == usuario.UserId);

            if (profile != null)
            {
                var profileDto = new MiniProfileDto
                {                    
                    UserName = profile.UserName,
                    Email = profile.Email,
                    ProfilePhoto = profile.ProfilePhoto
                };

                return Ok(profileDto);
            }
            else
            {
                return NotFound("No se encontró un perfil para el UsuarioId especificado.");
            }
        }


    }
}
