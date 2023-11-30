using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinterestAPI.Models;
using static PinterestAPI.Controllers.Pins.CreatesPinsController;

namespace PinterestAPI.Controllers.Pins
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SavePinsController : ControllerBase
    {
        private readonly PinterestContext _context;

        public SavePinsController(PinterestContext context)
        {
            _context = context;
        }

        public class SavePinDto
        {
            public int UserId { get; set; }
            public int PinId { get; set; }
        }


        [HttpPost("save")]
        public async Task<IActionResult> PostSave(SavePinDto savePinDto)
        {
            if (savePinDto == null)
            {
                return BadRequest();
            }

            // Verificar si ya existe la combinación de PinId y UserId
            var existingSave = _context.Saveds
                .FirstOrDefault(s => s.UserId == savePinDto.UserId && s.PinId == savePinDto.PinId);

            if (existingSave != null)
            {
                // Ya existe la combinación, puedes devolver un código de estado conflict
                return Conflict("Este pin ya esta guardado.");
            }

            var savePin = new Saved
            {
                UserId = savePinDto.UserId,
                PinId = savePinDto.PinId
            };

            _context.Saveds.Add(savePin);
            await _context.SaveChangesAsync();

            return Ok();
        }


        public class PinImageDto
        {
            public int PinId { get; set; }
            public byte[] Image { get; set; }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<PinDto>>> GetPinsInSaveds(int userId)
        {
            var saveds = await _context.Saveds
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
