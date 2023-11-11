using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinterestAPI.Models;
using static PinterestAPI.Controllers.Users.FollowUsersController;

namespace PinterestAPI.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollwersUsersController : ControllerBase
    {
        private readonly PinterestContext _context;

        public FollwersUsersController (PinterestContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFollowers(int id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null)
            {
                return BadRequest("El UsuarioId especificado no existe.");
            }

            var followerUsernames = _context.Followers
                .Where(f => f.UserFollowingId == id)
                .Join(
                    _context.Profiles,
                    follower => follower.UserFollowerId,
                    profile => profile.UserId,
                    (follower, profile) => profile.UserName
                )
                .ToList();

            return Ok(followerUsernames);
        }
    }
}
