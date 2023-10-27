using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinterestAPI.Models;


namespace PinterestAPI.Controllers.Pins
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchsController : ControllerBase
    {
        private readonly PinterestContext _context;

        public SearchsController(PinterestContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IEnumerable<Pin>> SearchPins(string searchTerm)
        {
            // Divide el término de búsqueda en palabras clave
            var keywords = searchTerm.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Realiza la consulta utilizando LINQ
            var query = _context.Pins.AsEnumerable()
            .Where(pin => keywords.Any(keyword =>
            (pin.Title ?? "").ToLower().Contains(keyword) ||
            (pin.Description ?? "").ToLower().Contains(keyword) ||
            (pin.AltText ?? "").ToLower().Contains(keyword)));



            // Verifica si la consulta devuelve resultados o no
            var results = query.ToList();

            // Si no se encontraron resultados, puedes devolver una lista vacía o null
            return results.Any() ? results : null;

        }

    }
}
