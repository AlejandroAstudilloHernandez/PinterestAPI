using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinterestAPI.Models;
using static PinterestAPI.Controllers.Users.FollowUsersController;

namespace PinterestAPI.Controllers.Boards
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FollowBoardsController : ControllerBase
    {
        private readonly PinterestContext _context;

        public FollowBoardsController (PinterestContext context)
        {
            _context = context;
        }

        public class FollowDto
        {
            public int UserId { get; set; }
            public int BoardId { get; set; }
        }

        [HttpPost("follow")]
        public async Task<IActionResult> PostFollow(FollowDto followDto)
        {

            if (ModelState.IsValid)
            {

                var usuario = await _context.Users.FindAsync(followDto.UserId);
                if (usuario == null)
                {
                    return BadRequest("El UsuarioId especificado no existe.");
                }
                var board = await _context.Boards.FindAsync(followDto.BoardId);
                if(board == null)
                {
                    return BadRequest("El Board especificado no existe.");
                }

                var followboard = new FollowBoard
                {
                    UserId = followDto.UserId,
                    BoardId = followDto.BoardId
                };

                _context.FollowBoards.Add(followboard);
                await _context.SaveChangesAsync();

                return Ok("Board seguido con éxito");
            }

            return BadRequest(ModelState);
        }


        [HttpDelete("unfollow")]
        public async Task<IActionResult> DeleteUnfollow(FollowDto followDto)
        {

            if (ModelState.IsValid)
            {

                var usuario = await _context.Users.FindAsync(followDto.UserId);
                if (usuario == null)
                {
                    return BadRequest("El UsuarioId especificado no existe.");
                }
                var board = await _context.Boards.FindAsync(followDto.BoardId);
                if (board == null)
                {
                    return BadRequest("El Board especificado no existe.");
                }
                var registroAEliminar = _context.FollowBoards.FirstOrDefault(e => e.UserId == followDto.UserId && e.BoardId == followDto.BoardId);
                _context.FollowBoards.Remove(registroAEliminar);
                await _context.SaveChangesAsync();

                return Ok("Board seguido con éxito");
            }

            return BadRequest(ModelState);
        }
    }
}
