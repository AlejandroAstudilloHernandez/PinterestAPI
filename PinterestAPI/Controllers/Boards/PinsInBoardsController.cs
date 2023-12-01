using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinterestAPI.Models;
using Microsoft.AspNetCore.Cors;
using static PinterestAPI.Controllers.Pins.CreatesPinsController;

namespace PinterestAPI.Controllers.Boards
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PinsInBoardsController : ControllerBase
    {
        private PinterestContext _context;

        public PinsInBoardsController (PinterestContext context)
        {
            _context = context;
        }

        public class PinImageDto
        {
            public int PinId { get; set; }
            public byte[] Image { get; set; }
        }

        [HttpGet("{boardId}")]
        public async Task<ActionResult<IEnumerable<PinDto>>> GetPinsInBoard(int boardId)
        {
            var pinBoardAssociations = await _context.PinBoardAssociations
                .Where(pa => pa.BoardId == boardId)
                .ToListAsync();

            if (pinBoardAssociations.Count == 0)
            {
                return NotFound("No se encontraron pines para el board especificado.");
            }

            var pinIds = pinBoardAssociations.Select(pa => pa.PinId).ToList();

            var pinsInBoard = await _context.Pins
                .Where(pin => pinIds.Contains(pin.PinId))
                .ToListAsync();

            if (pinsInBoard.Count == 0)
            {
                return NotFound("No se encontraron pines para el board especificado.");
            }

            // Puedes mapear los pines a un DTO si es necesario
            var pinImages = pinsInBoard
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
