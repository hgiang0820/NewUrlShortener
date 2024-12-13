using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerService.Data;
using UrlShortenerService.Models;

namespace UrlShortenerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        private readonly UrlShortenerServiceContext _context;

        public UrlsController(UrlShortenerServiceContext context)
        {
            _context = context;
        }

        // GET: api/Urls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Url>>> GetUrl()
        {
            return await _context.Url.ToListAsync();
        }

        // GET: api/Urls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Url>> GetUrl(int id)
        {
            var url = await _context.Url.FindAsync(id);

            if (url == null)
            {
                return NotFound();
            }

            return url;
        }

        // PUT: api/Urls/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUrl(int id, Url url)
        {
            if (id != url.Id)
            {
                return BadRequest();
            }

            _context.Entry(url).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UrlExistsId(id))
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
        public class UrlRequest
        {
            public string originalUrl { get; set; }
        }
        private string GenerateShortCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 6);
        }
        // POST: api/Urls
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<string>> PostUrl([FromBody] UrlRequest request)
        {
            if (string.IsNullOrEmpty(request.originalUrl))
            {
                return BadRequest("Original url cannot be null or empty.");
            }

            if (UrlExistsUrl(request.originalUrl))
            {
                return BadRequest("Original url is existed");
            }

            var new_url = new Url()
            {
                OriginalUrl = request.originalUrl,
                ShortCode = GenerateShortCode(),
                CreatedAt = DateTime.Now
            };

            _context.Url.Add(new_url);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUrl", new { id = new_url.Id }, new_url);
        }
        

        // DELETE: api/Urls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrl(int id)
        {
            var url = await _context.Url.FindAsync(id);
            if (url == null)
            {
                return NotFound();
            }

            _context.Url.Remove(url);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UrlExistsUrl(string url)
        {
            return _context.Url.Any(e => e.OriginalUrl == url);
        }
        private bool UrlExistsId(int id)
        {
            return _context.Url.Any(e => e.Id == id);
        }

        // CHECK LAI
        [HttpGet("redirect/{shortCode}")]
        public async Task<IActionResult> GetOriginalLink(string shortCode)
        {
            if (string.IsNullOrEmpty(shortCode))
            {
                return BadRequest("Short code cannot be null or empty.");
            }

            // Find the original link associated with the provided shorted link
            var shortedLink = await _context.Url.FirstOrDefaultAsync(m => m.ShortCode == shortCode);

            if (shortedLink == null)
            {
                return NotFound("The provided shorted link does not exist.");
            }

            // Return the original link
            return Ok(shortedLink.OriginalUrl);
        }
    }
}
