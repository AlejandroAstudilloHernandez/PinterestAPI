using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinterestAPI.Models;
using static PinterestAPI.Controllers.Users.FollowUsersController;

namespace PinterestAPI.Controllers.Boards
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddPinToBoardsController : ControllerBase
    {
        private readonly PinterestContext _context;

        public AddPinToBoardsController (PinterestContext context)
        {
            _context = context;
        }

        public class AddPinDto
        {
            public int PinId { get; set; }
            public int BoardId { get; set; }
        }

        [HttpPost("addPin")]
        public async Task<IActionResult> PostAddPinToBoard(AddPinDto addPinDto)
        {

            if (ModelState.IsValid)
            {

                var pin = await _context.Pins.FindAsync(addPinDto.PinId);
                if (pin == null)
                {
                    return BadRequest("El pin especificado no existe.");
                }
                var board = await _context.Boards.FindAsync(addPinDto.BoardId);
                if (board == null)
                {
                    return BadRequest("El usuario que desea seguir no existe.");
                }

                var addPin = new PinBoardAssociation
                {
                    PinId = addPinDto.PinId,
                    BoardId = board.BoardId
                };

                _context.PinBoardAssociations.Add(addPin);
                await _context.SaveChangesAsync();

                return Ok("Pin añadido con éxito");
            }

            return BadRequest(ModelState);
        }
    }
}
