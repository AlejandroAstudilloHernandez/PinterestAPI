using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinterestAPI.Models;

namespace PinterestAPI.Controllers.Boards
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BoardsByUsersController : ControllerBase
    {
        private PinterestContext _context;

        public BoardsByUsersController (PinterestContext context)
        {
            _context = context;
        }

        [HttpGet("boardsByUser/{userId}")]
        public IActionResult GetBoardsByUser(int userId)
        {
            // Busca los Boards para el UserId dado
            var boardsByUser = _context.Boards
                .Where(board => board.UserId == userId)
                .ToList();

            if (boardsByUser.Count == 0)
            {
                return NotFound("No se encontraron tableros para el usuario especificado.");
            }

            return Ok(boardsByUser);
        }
    }
}
