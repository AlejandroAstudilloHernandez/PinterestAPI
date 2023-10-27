using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinterestAPI.Models;
using static PinterestAPI.Controllers.Pins.SavePinsController;

namespace PinterestAPI.Controllers.Pins
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly PinterestContext _context;

        public CommentsController(PinterestContext context)
        {
            _context = context;
        }

        public class CommentPinDto
        {
            public string Comment { get; set; }
            public int PinId { get; set; }
        }

        [HttpPost("comment")]
        public async Task<IActionResult> PostComment(CommentPinDto commentPinDto)
        {
            // Verifica si el objeto commentPinDto es nulo y devuelve un BadRequest si lo es.
            if (commentPinDto == null)
            {
                return BadRequest();
            }

            // Crea una nueva instancia de la entidad Comment a partir de los datos en commentPinDto.
            var commentPin = new Comment
            {
                Comment1 = commentPinDto.Comment,
                PinId = commentPinDto.PinId
            };

            // Agrega el comentario recién creado al contexto de Entity Framework.
            _context.Comments.Add(commentPin);

            // Guarda los cambios en la base de datos. Esta operación es asincrónica.
            await _context.SaveChangesAsync();

            // Responde con un Ok() como confirmación de que el comentario se agregó con éxito.
            return Ok();
        }


        [HttpDelete("comment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
            {
                return NotFound(); // El comentario no se encontró, puedes devolver una respuesta NotFound
            }

            _context.Comments.Remove(comment); // Elimina el comentario de la base de datos
            await _context.SaveChangesAsync(); // Guarda los cambios

            return NoContent(); // Respuesta exitosa
        }
    }
}
