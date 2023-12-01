using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinterestAPI.Models;
using Microsoft.AspNetCore.Cors;
using static PinterestAPI.Controllers.Boards.DeleteBoardsController;

namespace PinterestAPI.Controllers.Pins
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeletePinsController : ControllerBase
    {
        private PinterestContext _context;

        public DeletePinsController(PinterestContext context)
        {
            _context = context;
        }

        public class DeletePinDto
        {
            public int PinId { get; set; }
            public int UserId { get; set; }
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeletePin(DeletePinDto deletePinDto)
        {
            var user = await _context.Users.FindAsync(deletePinDto.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var board = await _context.Pins.FindAsync(deletePinDto.PinId);
            if (board == null)
            {
                return NotFound();
            }
            // 1. Eliminar registros relacionados en la tabla "Saveds"
            var savePinsAEliminar = _context.Saveds.Where(fb => fb.PinId == deletePinDto.PinId);
            _context.Saveds.RemoveRange(savePinsAEliminar);

            // 2. Eliminar el tablero en la tabla "Boards"
            var registroAEliminar = _context.Pins.FirstOrDefault(e => e.PinId == deletePinDto.PinId && e.UserId == deletePinDto.UserId);
            _context.Pins.Remove(registroAEliminar);

            await _context.SaveChangesAsync();



            return NoContent();
        }
    }
}
