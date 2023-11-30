using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinterestAPI.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace PinterestAPI.Controllers.Pins
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        public class PinImageDto
        {
            public int PinId { get; set; }
            public byte[] Image { get; set; }
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
            var localDateTime = DateTime.Now;
            var fechaLegible = localDateTime.ToString("dd/MM/yyyy");
            var utcDateTime = localDateTime.ToUniversalTime();

            if (ModelState.IsValid)
            {
                var imageBytes = await ComprimirImagen(pinDto.Image);

                var usuario = await _context.Users.FindAsync(pinDto.UserId);
                if (usuario == null)
                {
                    return BadRequest("El UsuarioId especificado no existe.");
                }

                if (pinDto.SensitiveContent == null)
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

        private async Task<byte[]> ComprimirImagen(IFormFile archivoImagen)
        {
            using var ms = new MemoryStream();
            await archivoImagen.CopyToAsync(ms);

            // Cargar la imagen utilizando ImageSharp
            using var imagen = Image.Load(ms.ToArray());

            // Redimensionar la imagen si es necesario
            imagen.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(800, 600), // Ajusta según tus requisitos de tamaño
                Mode = ResizeMode.Max
            }));

            // Comprimir la imagen con calidad específica
            imagen.Save(ms, new JpegEncoder
            {
                Quality = 80 // Ajusta la calidad de compresión
            });

            return ms.ToArray();
        }


        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<PinDto>>> GetUserPins(int userId)
        {
            var saveds = await _context.Pins
                .Where(pa => pa.UserId == userId)
                .ToListAsync();

            if (saveds.Count == 0)
            {
                return NotFound("No hay pines guardados.");
            }

            var pinIds = saveds.Select(pa => pa.PinId).ToList();

            var pinsInSaveds = await _context.Pins
                .Where(pin => pinIds.Contains(pin.PinId))
                .ToListAsync();

            if (pinsInSaveds.Count == 0)
            {
                return NotFound("No hay pines guardados.");
            }

            // Puedes mapear los pines a un DTO si es necesario
            var pinImages = pinsInSaveds
                .Select(pin => new PinImageDto
                {
                    PinId = pin.PinId,
                    Image = pin.Image
                })
                .ToList();

            return Ok(pinImages);
        }



    }
}
