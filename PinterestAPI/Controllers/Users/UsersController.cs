using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinterestAPI.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace PinterestAPI.Controllers.Users
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly PinterestContext _context;

        public UsersController(PinterestContext context)
        {
            _context = context;
        }


        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }



        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAndAssociatedData(int userId)
        {
            // Busca el usuario por Id incluyendo las colecciones relacionadas
            var user = await _context.Users
                .Include(u => u.Boards)
                .Include(u => u.FollowBoards)
                .Include(u => u.FollowerUserFollowers)
                .Include(u => u.FollowerUserFollowings)
                .Include(u => u.Pins)
                .Include(u => u.Profiles)
                .Include(u => u.Saveds)
                .Include(u => u.Comments)
                .Include(u => u.Replies)
                .Include(u => u.PinBoardAssociations)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Elimina los datos asociados al usuario

            _context.RemoveRange(user.Replies);
            _context.RemoveRange(user.Comments);
            _context.RemoveRange(user.Saveds);
            _context.RemoveRange(user.Boards);
            _context.RemoveRange(user.Pins);
            _context.RemoveRange(user.PinBoardAssociations);
            _context.RemoveRange(user.FollowBoards);            
            _context.RemoveRange(user.FollowerUserFollowers);
            _context.RemoveRange(user.FollowerUserFollowings);
            _context.RemoveRange(user.Profiles);

            // Elimina el usuario
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return Ok("Usuario y datos asociados eliminados correctamente.");
        }



        #region Metodos
        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        private bool UserExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }

        #endregion
    }
}
