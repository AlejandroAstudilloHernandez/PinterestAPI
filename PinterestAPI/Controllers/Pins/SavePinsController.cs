using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinterestAPI.Models;
using static PinterestAPI.Controllers.Pins.CreatesPinsController;

namespace PinterestAPI.Controllers.Pins
{
    [Route("api/[controller]")]
    [ApiController]
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
            
            if (savePinDto == null) {  return BadRequest(); }

            var savePin = new Saved
            {
                UserId = savePinDto.UserId,
                PinId = savePinDto.PinId
            };            

            _context.Saveds.Add(savePin);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
