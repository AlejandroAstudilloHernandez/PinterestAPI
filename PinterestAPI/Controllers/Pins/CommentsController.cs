using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PinterestAPI.Models;
using Microsoft.AspNetCore.Cors;
using static PinterestAPI.Controllers.Pins.CommentsController;
using static PinterestAPI.Controllers.Pins.SavePinsController;

namespace PinterestAPI.Controllers.Pins
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly PinterestContext _context;

        public CommentsController(PinterestContext context)
        {
            _context = context;
        }

        public class CommentPinDto
        {
            public int UserId { get; set; }
            public string Comment { get; set; }
            public int PinId { get; set; }
            
        }

        public class CommentsPinDto
        {
            public int CommentId { get; set; }
            public int UserId { get; set; }
            public string Comment { get; set; }
            public int PinId { get; set; }
            public byte[] ProfilePhoto { get; set; }
            public string Username { get; set; }
        }

        [HttpPost("comment")]
        public async Task<IActionResult> PostComment(CommentPinDto commentPinDto)
        {
            // Verifica si el objeto commentPinDto es nulo y devuelve un BadRequest si lo es.
            if (commentPinDto == null)
            {
                return BadRequest();
            }
            var user = await _context.Users.FindAsync(commentPinDto.UserId);
            if (user == null)
            {
                return BadRequest("El usuario no existe.");
            }            

            // Crea una nueva instancia de la entidad Comment a partir de los datos en commentPinDto.
            var commentPin = new Comment
            {
                UserId = commentPinDto.UserId,
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


        [HttpDelete("{commentId}")]
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

        [HttpGet("{pinId}")]
        public IActionResult GetCommentsForPin(int pinId)
        {
            // Realiza la consulta para obtener todos los comentarios que pertenecen al PinId proporcionado.
            var comments = _context.Comments
                .Where(c => c.PinId == pinId)
                .ToList();

            // Lista para almacenar los comentarios con la información del perfil.
            var commentsWithProfileInfo = new List<CommentsPinDto>();

            foreach (var comment in comments)
            {
                // Realiza una consulta para obtener la información del perfil basada en el UserId de cada comentario.
                var profilePhoto = _context.Profiles
                    .Where(p => p.UserId == comment.UserId)
                    .Select(p => p.ProfilePhoto)
                    .FirstOrDefault();

                var username = _context.Profiles
                    .Where(p => p.UserId == comment.UserId)
                    .Select(p => p.UserName)
                    .FirstOrDefault();

                // Agrega un objeto CommentsPinDto a la lista.
                commentsWithProfileInfo.Add(new CommentsPinDto
                {
                    CommentId = comment.CommentId,
                    UserId = comment.UserId,
                    Comment = comment.Comment1,
                    PinId = comment.PinId,
                    ProfilePhoto = profilePhoto,
                    Username = username
                });
            }

            // Devuelve la lista de CommentsPinDto.
            return Ok(commentsWithProfileInfo);
        }


    }
}
