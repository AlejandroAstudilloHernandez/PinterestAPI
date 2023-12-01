using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinterestAPI.Models;
using Microsoft.AspNetCore.Cors;
using static PinterestAPI.Controllers.Users.FollowUsersController;

namespace PinterestAPI.Controllers.Users
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlockUsersController : ControllerBase
    {
        private readonly PinterestContext _context;

        public BlockUsersController(PinterestContext context)
        {
            _context = context;
        }

        public class BlockUserDto
        {
            public int BlockingUserId { get; set; }
            public int BlockedUserId { get; set; }
        }

        [HttpPost("addPin")]
        public async Task<IActionResult> PostBlockUser(BlockUserDto blockUserDto)
        {
            var localDateTime = DateTime.Now;  // obtiene la hora local actual
            var fechaLegible = localDateTime.ToString("dd/MM/yyyy");  // Formatea la fecha como "día/mes/año"
            var utcDateTime = localDateTime.ToUniversalTime();  // convierte la hora local a UTC

            var usuario = await _context.Users.FindAsync(blockUserDto.BlockingUserId);
            if (usuario == null)
            {
                return BadRequest("El UsuarioId especificado no existe.");
            }
            var usuario2 = await _context.Users.FindAsync(blockUserDto.BlockedUserId);
            if (usuario2 == null)
            {
                return BadRequest("El UsuarioId especificado no existe.");
            }

            var blockUser = new Block
            {
                BlockingUserId = blockUserDto.BlockingUserId,
                BlockedUserId = blockUserDto.BlockedUserId,
                BlockDate = utcDateTime
            };

            _context.Blocks.Add(blockUser);
            await _context.SaveChangesAsync();
            return Ok("Usuario Bloqueado.");

        }        
    }
}
