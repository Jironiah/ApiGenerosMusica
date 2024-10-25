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
            new Generos { Id = 1, NombreGenero="Rock" }
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
        public ActionResult Put([FromBody] Generos updatedProduct)
        {
            var product = generos.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (product == null)
            {
                return NotFound();
            }

            product.NombreGenero = updatedProduct.NombreGenero;

            return NoContent();
        }

        // POST: api/Generos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/Post")]
        [HttpPost]
        public ActionResult<Generos> Post(Generos genero)
        {
            genero.Id = generos.Count + 1;
            generos.Add(genero);
            //var jsonData = JsonSerializer.Serialize(generos, new JsonSerializerOptions { WriteIndented = true });

            return CreatedAtAction(nameof(Get), new { id = genero.Id }, genero);
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
