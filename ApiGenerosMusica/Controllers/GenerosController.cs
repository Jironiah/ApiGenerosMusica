using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiGenerosMusica.Data;
using ApiGenerosMusica.Models;
using System.Text.Json;
using System.Text;

namespace ApiGenerosMusica.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly ApiGenerosMusicaContext _context;

        private static List<Generos> generos = new List<Generos>
        {
            // new Generos { Id = 1, NombreGenero="Rock", ImagenBase64= "Aqui va el base64 de ejemplo"}
        };

        public GenerosController(ApiGenerosMusicaContext context)
        {
            _context = context;
        }

        // GET: api/Generos
        [Route("api/Get")]
        [HttpGet]
        public ActionResult<IEnumerable<Generos>> GetGeneros()
        {
            return Ok(generos);
        }

        // GET: api/Generos/5
        [Route("api/Get/{id}")]
        [HttpGet]
        public ActionResult<Generos> Get(int id)
        {
            var genero = generos.FirstOrDefault(p => p.Id == id);
            if (genero == null)
            {
                return NotFound();
            }
            return Ok(genero);
        }

        // PUT: api/Generos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/Put")]
        [HttpPut]
        public ActionResult Put([FromBody] Generos genero)
        {
            var generoExistente = generos.FirstOrDefault(p => p.Id == genero.Id);
            if (generoExistente == null)
            {
                return NotFound();
            }

            generoExistente.NombreGenero = genero.NombreGenero;

            // Actualizar la imagen si se envía en base64
            if (!string.IsNullOrEmpty(genero.ImagenBase64))
            {
                generoExistente.Imagen = Convert.FromBase64String(genero.ImagenBase64);
            }

            return NoContent();
        }


        // POST: api/Generos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/Post")]
        [HttpPost]
        public ActionResult<Generos> Post([FromBody] Generos genero)
        {
            try
            {
                // Convertir la imagen de base64 a byte[] si está presente
                if (!string.IsNullOrEmpty(genero.ImagenBase64))
                {
                    // Asegúrate de que la cadena es válida antes de intentar convertirla
                    try
                    {
                        genero.Imagen = Convert.FromBase64String(genero.ImagenBase64);
                    }
                    catch (FormatException ex)
                    {
                        // Aquí puedes manejar el error adecuadamente
                        Console.WriteLine($"Error al convertir Base64: {ex.Message}");
                        return BadRequest("La cadena Base64 no es válida.");
                    }
                }

                // Verificar si el objeto recibido es nulo
                if (genero == null)
                {
                    return BadRequest("El objeto 'genero' es nulo.");
                }

                // Convertir la imagen de base64 a byte[] si está presente
                if (!string.IsNullOrEmpty(genero.ImagenBase64))
                {
                    genero.Imagen = Convert.FromBase64String(genero.ImagenBase64);
                }

                // Generar un nuevo ID y agregar el género a la lista
                genero.Id = generos.Count + 1;
                generos.Add(genero);

                return CreatedAtAction(nameof(Get), new { id = genero.Id }, genero);
            }
            catch (Exception ex)
            {
                // Log el error (puedes usar un logger o simplemente escribir en la consola)
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        // DELETE: api/Generos/5
        [Route("api/Delete/{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var genero = generos.FirstOrDefault(p => p.Id == id);
            if (genero == null)
            {
                return NotFound();
            }

            generos.Remove(genero);
            return NoContent();
        }

        private bool GenerosExists(int id)
        {
            return generos.Any(a => a.Id == id);
        }

        [HttpGet("download")]
        public IActionResult DownloadJson()
        {
            // Convertir la llista de productes a JSON utilitzant System.Text.Json
            var jsonData = JsonSerializer.Serialize(generos, new JsonSerializerOptions { WriteIndented = true });

            // Convertir el JSON a bytes per a la descàrrega
            var byteArray = Encoding.UTF8.GetBytes(jsonData);
            var stream = new MemoryStream(byteArray);

            // Retornar el fitxer JSON com a descàrrega
            return File(stream, "application/json", "products.json");
        }
    }
}
