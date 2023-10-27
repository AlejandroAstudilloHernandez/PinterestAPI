using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PinterestAPI.Models;
using static PinterestAPI.Controllers.Pins.CreatesPinsController;

namespace PinterestAPI.Controllers.Pins
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionsController : ControllerBase
    {
        private readonly PinterestContext _context;
        public ReactionsController(PinterestContext context)
        {
            _context = context;
        }


        [HttpPut("add")]
        public async Task<IActionResult> PutAddReaction(int pinId, int reactionType)
        {
            var pin = await _context.Pins.FindAsync(pinId);
            var reaction = _context.Reactions.FirstOrDefault(r => r.PinId == pinId);

            if (pin == null)
            {
                return NotFound();
            }
            if (reaction == null)
            {
                return NotFound();
            }
            if (reactionType > 4 || reactionType < 0)
            {
                return BadRequest("Reaccion invalida.");
            }

            try
            {
                switch (reactionType)
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

                return Ok(pin);
            }
            catch (DbUpdateException)
            {
                if (reactionType > 4 || reactionType < 0)
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
        public async Task<IActionResult> PutRemoveReaction(int pinId, int reactionType)
        {
            var pin = await _context.Pins.FindAsync(pinId);
            var reaction = _context.Reactions.FirstOrDefault(r => r.PinId == pinId);

            if (pin == null)
            {
                return NotFound();
            }
            if (reaction == null)
            {
                return NotFound();
            }
            if (reactionType > 4 || reactionType < 0)
            {
                return BadRequest("Reaccion invalida.");
            }

            try
            {
                switch (reactionType)
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

                return Ok(pin);
            }
            catch (DbUpdateException)
            {
                if (reactionType > 4 || reactionType < 0)
                {
                    return BadRequest("Reaccion invalida.");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
