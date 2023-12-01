using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinterestAPI.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;

namespace PinterestAPI.Controllers.Users
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly PinterestContext _context;

        public RegisterController(PinterestContext context)
        {
            _context = context;
        }


        // POST: api/Register
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {            

            if (UserExists(user.Email))
            {
                return Conflict("Usuario ya existente.");
            }

            // Guarda el User para que Entity Framework Core lo inserte
            user.Pass = Encrypt.GetSHA256(user.Pass.ToString());
            _context.Users.Add(user);

            try
            {
                // Guarda los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Después de la inserción, obtén el UserId
                var insertedUserId = user.UserId;

                // Obtiene el UserName y Birthday del usuario recién insertado
                string[] newUsername = user.Email.Split("@");
                var newMail = user.Email;
                var newBirthday = user.Birthday; // Asegúrate de que la propiedad Birthday esté configurada correctamente en tu modelo

                // Crea un nuevo perfil y agrégalo a la tabla "Profiles"
                var newProfile = new Profile
                {
                    Name = newUsername[0],
                    UserName = newUsername[0],
                    UserId = insertedUserId,
                    Birthday = newBirthday,
                    Email = newMail,
                    Privacy = false
                };

                _context.Profiles.Add(newProfile);
                await _context.SaveChangesAsync();

                // Configura las opciones de serialización JSON
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    // Otras configuraciones opcionales
                };

                // Serializa el objeto user y devuelve la respuesta
                var jsonUser = JsonSerializer.Serialize(user, jsonSerializerOptions);

                return Ok("Registro Existoso!");
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Email))
                {
                    return Conflict("Usuario ya existente.");
                }
                else
                {
                    throw;
                }
            }
        }

        private bool UserExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }

    }
}
