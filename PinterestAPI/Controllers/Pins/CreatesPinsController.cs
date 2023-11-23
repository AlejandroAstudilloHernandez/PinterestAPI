using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinterestAPI.Models;

namespace PinterestAPI.Controllers.Pins
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatesPinsController : ControllerBase
    {
        private readonly PinterestContext _context;
        public CreatesPinsController(PinterestContext context)
        {
            _context = context;
        }

        public class PinDto
        {
            public IFormFile Image { get; set; } = null!;

            public string Title { get; set; } = null!;

            public string? Description { get; set; }

            public string? AltText { get; set; }

            public string? Link { get; set; }

            public int UserId { get; set; }

            public string? ImageUrl { get; set; }

            public bool? SensitiveContent { get; set; }
        }

        //Metodo para convertir la imagen
        private async Task<byte[]> ObtenerBytesImagen(IFormFile archivoImagen)
        {
            using var ms = new MemoryStream();
            await archivoImagen.CopyToAsync(ms);
            return ms.ToArray();
        }

        [HttpPost("create")]
        public async Task<IActionResult> PostCreate([FromForm] PinDto pinDto)
        {

            var localDateTime = DateTime.Now;  // obtiene la hora local actual
            var fechaLegible = localDateTime.ToString("dd/MM/yyyy");  // Formatea la fecha como "día/mes/año"
            var utcDateTime = localDateTime.ToUniversalTime();  // convierte la hora local a UTC
            if (ModelState.IsValid)
            {
                var imageBytes = await ObtenerBytesImagen(pinDto.Image);                

                var usuario = await _context.Users.FindAsync(pinDto.UserId);
                if (usuario == null)
                {
                    return BadRequest("El UsuarioId especificado no existe.");
                }
                if(pinDto.SensitiveContent == null)
                {
                    pinDto.SensitiveContent = false;
                }

                var pin = new Pin
                {
                    Image = imageBytes,
                    Title = pinDto.Title,
                    Description = pinDto.Description,
                    AltText = pinDto.AltText,
                    Link = pinDto.Link,
                    UserId = pinDto.UserId,
                    Date = utcDateTime,
                    SensitiveContent = pinDto.SensitiveContent
                };

                _context.Pins.Add(pin);
                await _context.SaveChangesAsync();

                var newReactions = new Reaction
                {
                    GoodIdeaReaction = 0,
                    LoveReaction = 0,
                    ThanksReaction = 0,
                    WowReaction = 0,
                    HahaReaction = 0,
                    PinId = pin.PinId
                };
                _context.Reactions.Add(newReactions);
                await _context.SaveChangesAsync();

                return Ok(pin);
            }

            return BadRequest(ModelState);
        }



    }
}
