using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinterestAPI.Models;

namespace PinterestAPI.Controllers.Pins
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SinglePinsController : ControllerBase
    {
        private readonly PinterestContext _context;
        public SinglePinsController(PinterestContext context)
        {
            _context = context;
        }

                
        [HttpGet("{id}")]
        public async Task<ActionResult<Pin>> GetUser(int id)
        {
          if (_context.Pins == null)
          {
              return NotFound();
          }
            var pin = await _context.Pins.FindAsync(id);

            if (pin == null)
            {
                return NotFound();
            }

            return pin;
        }
         
    }
}
