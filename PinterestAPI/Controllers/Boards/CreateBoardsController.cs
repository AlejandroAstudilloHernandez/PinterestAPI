using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinterestAPI.Models;
using static PinterestAPI.Controllers.Pins.CreatesPinsController;

namespace PinterestAPI.Controllers.Boards
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateBoardsController : ControllerBase
    {
        private readonly PinterestContext _context;

        public CreateBoardsController(PinterestContext context)
        {
            _context = context;
        }

        public class BoardDto
        {
            public string BoardName { get; set; }
            public int UserId { get; set;}
        }

        [HttpPost("create")]
        public async Task<IActionResult> PostCreate(BoardDto boardDto)
        {
            var localDateTime = DateTime.Now;  // obtiene la hora local actual
            var fechaLegible = localDateTime.ToString("dd/MM/yyyy");  // Formatea la fecha como "día/mes/año"
            var utcDateTime = localDateTime.ToUniversalTime();  // convierte la hora local a UTC
            if (ModelState.IsValid)
            {

                var usuario = await _context.Users.FindAsync(boardDto.UserId);
                if (usuario == null)
                {
                    return BadRequest("El UsuarioId especificado no existe.");
                }

                var board = new Board
                {
                    BoardName = boardDto.BoardName,
                    BoardDescription = "",
                    UserId = boardDto.UserId,
                    Date = utcDateTime
                };

                _context.Boards.Add(board);
                await _context.SaveChangesAsync();                

                return Ok("Board creado con éxito");
            }

            return BadRequest(ModelState);
        }

    }
}
