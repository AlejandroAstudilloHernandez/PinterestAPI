using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinterestAPI.Models;
using System.Linq.Dynamic.Core;

namespace PinterestAPI.Controllers.Pins
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchsController : ControllerBase
    {
        private readonly PinterestContext _context;

        public SearchsController(PinterestContext context)
        {
            _context = context;
        }

        public class PinImageDto
        {
            public int PinId { get; set; }
            public byte[] Image { get; set; }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Pin>>> SearchPins(string searchTerm)
        {
            // Divide el término de búsqueda en palabras clave
            var keywords = searchTerm.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Realiza la consulta utilizando LINQ
            var query = _context.Pins.AsEnumerable()
        .Where(pin => keywords.Any(keyword =>
            (pin.Title ?? "").ToLower().Contains(keyword) ||
            (pin.Description ?? "").ToLower().Contains(keyword) ||
            (pin.AltText ?? "").ToLower().Contains(keyword)))
        .Select(pin => new PinImageDto
        {
            PinId = pin.PinId,
            Image = pin.Image // Ajusta esta propiedad según la propiedad real de tu entidad Pin
        });

            // Si no se encontraron resultados, puedes devolver una lista vacía o null
            return Ok(query);

        }

    }
}
