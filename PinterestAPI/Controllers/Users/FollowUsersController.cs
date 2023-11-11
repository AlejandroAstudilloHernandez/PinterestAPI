using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinterestAPI.Models;
using static PinterestAPI.Controllers.Boards.DeleteBoardsController;

namespace PinterestAPI.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowUsersController : ControllerBase
    {
        private readonly PinterestContext _context;

        public FollowUsersController (PinterestContext context)
        {
            _context = context;
        }

        public class FollowUserDto
        {
            public int UserId { get; set; }
            public int FollowingUserId { get; set; }
        }

        [HttpPost("follow")]
        public async Task<IActionResult> PostFollow(FollowUserDto followUserDto)
        {

            if (ModelState.IsValid)
            {

                var usuario = await _context.Users.FindAsync(followUserDto.UserId);
                if (usuario == null)
                {
                    return BadRequest("El UsuarioId especificado no existe.");
                }
                var following = await _context.Users.FindAsync(followUserDto.FollowingUserId);
                if (following == null)
                {
                    return BadRequest("El usuario que desea seguir no existe.");
                }

                var follower = new Follower
                {
                    UserFollowerId = followUserDto.UserId,
                    UserFollowingId = followUserDto.FollowingUserId
                };

                _context.Followers.Add(follower);
                await _context.SaveChangesAsync();

                return Ok("Board seguido con éxito");
            }

            return BadRequest(ModelState);
        }

        public class DeleteFollowDto
        {
            public int followerId { get; set; }
            public int UserFollowerId { get; set; }
            public int UserFollowingId { get; set; }
        }

        [HttpDelete("followDelete")]
        public async Task<IActionResult> DeleteBoard(DeleteFollowDto deleteFollowDto)
        {
            var user = await _context.Users.FindAsync(deleteFollowDto.UserFollowerId);
            if (user == null)
            {
                return NotFound();
            }
            var user2 = await _context.Users.FindAsync(deleteFollowDto.UserFollowingId);
            if (user == null)
            {
                return NotFound();
            }
            var Follow = await _context.Followers.FindAsync(deleteFollowDto.followerId);
            if (Follow == null)
            {
                return NotFound();
            }

            // Eliminar el registro de follower - following
            var registroAEliminar = _context.Followers.FirstOrDefault(e => e.UserFollowerId == deleteFollowDto.UserFollowerId && e.UserFollowingId == deleteFollowDto.UserFollowingId);
            _context.Followers.Remove(registroAEliminar);

            await _context.SaveChangesAsync();



            return NoContent();
        }

    }
}
