using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinterestAPI.Models;

namespace PinterestAPI.Controllers.Boards
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteBoardsController : ControllerBase
    {
        private readonly PinterestContext _context;
        
        public DeleteBoardsController(PinterestContext context)
        {
            _context = context;
        }

        public class DeleteBoardDto
        {
            public int BoardId { get; set; }
            public int UserId { get; set; }
        }
       
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBoard(DeleteBoardDto deleteBoardDto)
        {            
            var user = await _context.Users.FindAsync(deleteBoardDto.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var board = await _context.Boards.FindAsync(deleteBoardDto.BoardId);
            if (board == null)
            {
                return NotFound();
            }
            // 1. Eliminar registros relacionados en la tabla "FollowBoards"
            var followBoardRecords = _context.FollowBoards.Where(fb => fb.BoardId == deleteBoardDto.BoardId);
            _context.FollowBoards.RemoveRange(followBoardRecords);

            // 2. Eliminar el tablero en la tabla "Boards"
            var registroAEliminar = _context.Boards.FirstOrDefault(e => e.BoardId == deleteBoardDto.BoardId && e.UserId == deleteBoardDto.UserId);
            _context.Boards.Remove(registroAEliminar);

            await _context.SaveChangesAsync();



            return NoContent();
        }
    }
}
