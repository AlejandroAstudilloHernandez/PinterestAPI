using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinterestAPI.Models;
using System.Buffers.Text;

namespace PinterestAPI.Controllers.Pins
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomesPinsController : ControllerBase
    {        
        private readonly PinterestContext _context;
        public HomesPinsController(PinterestContext context)
        {
            _context = context;
        }

        public class PinImageDto
        {
            public int PinId { get; set; }
            public byte[] Image { get; set; }
        }

        // ...

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PinImageDto>>> GetPinImages()
        {

            // Obtén PinId e Image de los Pins y convierte Image a Base64
            var pinImages = await _context.Pins
                .Select(pin => new PinImageDto
                {
                    PinId = pin.PinId,
                    Image = pin.Image
                })
                .ToListAsync();

            return pinImages;
        }
    }
}
