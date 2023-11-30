using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PinterestAPI.Models;
using static PinterestAPI.Controllers.Pins.CreatesPinsController;
using Microsoft.AspNetCore.Authorization;

namespace PinterestAPI.Controllers.Pins
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReactionsController : ControllerBase
    {
        private readonly PinterestContext _context;
        public ReactionsController(PinterestContext context)
        {
            _context = context;
        }

        public class likesDto
        {
            public int pinId { get; set; }
            public int reactionType { get; set;}
        }


        [HttpPut("add")]
        public async Task<IActionResult> PutAddReaction(likesDto likes)
        {
            var pin = await _context.Pins.FindAsync(likes.pinId);
            var reaction = _context.Reactions.FirstOrDefault(r => r.PinId == likes.pinId);

            if (pin == null)
            {
                return NotFound();
            }
            if (reaction == null)
            {
                return NotFound();
            }
            if (likes.reactionType > 4 || likes.reactionType < 0)
            {
                return BadRequest("Reaccion invalida.");
            }

            try
            {
                switch (likes.reactionType)
                {
                    case 0:
                        reaction.GoodIdeaReaction++;
                        break;
                    case 1:
                        reaction.LoveReaction++; break;
                    case 2:
                        reaction.ThanksReaction++; break;
                    case 3:
                        reaction.WowReaction++; break;
                    case 4:
                        reaction.HahaReaction++; break;
                }
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateException)
            {
                if (likes.reactionType > 4 || likes.reactionType < 0)
                {
                    return BadRequest("Reaccion invalida.");
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPut("remove")]
        public async Task<IActionResult> PutRemoveReaction(likesDto likes)
        {
            var pin = await _context.Pins.FindAsync(likes.pinId);
            var reaction = _context.Reactions.FirstOrDefault(r => r.PinId == likes.pinId);

            if (pin == null)
            {
                return NotFound();
            }
            if (reaction == null)
            {
                return NotFound();
            }
            if (likes.reactionType > 4 || likes.reactionType < 0)
            {
                return BadRequest("Reaccion invalida.");
            }

            try
            {
                switch (likes.reactionType)
                {
                    case 0:
                        reaction.GoodIdeaReaction--;
                        break;
                    case 1:
                        reaction.LoveReaction--; break;
                    case 2:
                        reaction.ThanksReaction--; break;
                    case 3:
                        reaction.WowReaction--; break;
                    case 4:
                        reaction.HahaReaction--; break;
                }
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateException)
            {
                if (likes.reactionType > 4 || likes.reactionType < 0)
                {
                    return BadRequest("Reaccion invalida.");
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpGet("{pinId}")]
        public IActionResult GetReactionsForPin(int pinId)
        {
            // Realiza la consulta para obtener todas las reacciones que pertenecen al PinId proporcionado.
            var loveReaction = _context.Reactions
            .Where(r => r.PinId == pinId)
            .Select(r => r.LoveReaction)
            .FirstOrDefault();


            // Si no se encuentra ninguna reacción, puedes devolver una respuesta NotFound.
            if (loveReaction == null)
            {
                return NotFound();
            }

            // Mapea las reacciones a un DTO o simplemente devuelve el objeto Reaction.
            return Ok(loveReaction);
        }

    }
}
