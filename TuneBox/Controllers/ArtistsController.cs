using Microsoft.AspNetCore.Mvc;
using TuneBox.Models;
using TuneBox.Services;

namespace TuneBox.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistsController : ControllerBase
    {
        private readonly ArtistService _artistService;

        public ArtistsController(ArtistService artistService)
        {
            _artistService = artistService;
        }

        // GET
        [HttpGet]
        public ActionResult<IEnumerable<Artist>> GetAllArtists()
        {
            return Ok(_artistService.GetAllArtists());
        }

        // GET by ID
        [HttpGet("{id}")]
        public ActionResult<Artist> GetArtistById(int id)
        {
            var artist = _artistService.GetArtistById(id);
            if (artist == null)
            {
                return NotFound($"Artist with ID {id} not found.");
            }
            return Ok(artist);
        }

        // POST
        [HttpPost]
        public ActionResult<Artist> CreateArtist([FromBody] Artist newArtist)
        {
            var createdArtist = _artistService.CreateArtist(newArtist);
            return CreatedAtAction(nameof(GetArtistById), new { id = createdArtist.Id }, createdArtist);
        }

        // PUT
        [HttpPut("{id}")]
        public IActionResult UpdateArtist(int id, [FromBody] Artist updatedArtist)
        {
            var isUpdated = _artistService.UpdateArtist(id, updatedArtist);

            if (!isUpdated)
            {
                return NotFound($"Artist with ID {id} not found.");
            }

            return NoContent();
        }

        // DELETE by ID
        [HttpDelete("{id}")]
        public IActionResult DeleteArtist(int id)
        {
            var isDeleted = _artistService.DeleteArtist(id);

            if (!isDeleted)
            {
                return NotFound($"Artist with ID {id} not found.");
            }

            return NoContent();
        }

        // DELETE all
        [HttpDelete]
        public IActionResult DeleteAllArtists()
        {
            _artistService.DeleteAllArtists();
            return NoContent();
        }

        // GET songs by artist ID, one-to-many
        [HttpGet("{id}/songs")]
        public ActionResult<IEnumerable<Song>> GetSongsByArtist(int id)
        {
            var artistSongs = _artistService.GetSongsByArtist(id);

            if (artistSongs == null)
            {
                return NotFound($"Artist with ID {id} not found.");
            }

            return Ok(artistSongs);
        }
    }
}