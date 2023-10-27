using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinterestAPI.Models;

namespace PinterestAPI.Controllers.Pins
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomesPinsController : ControllerBase
    {        
        private readonly PinterestContext _context;
        public HomesPinsController(PinterestContext context)
        {
            _context = context;
        }

        //Pins para feed de inicio probicional
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pin>>> GetPins()
        {
            var random = new Random();
            var pins = await _context.Pins
                .ToListAsync();

            return pins;
        }        
    }
}
