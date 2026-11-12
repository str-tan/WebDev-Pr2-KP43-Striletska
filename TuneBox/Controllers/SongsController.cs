using Microsoft.AspNetCore.Mvc;
using TuneBox.Models;
using TuneBox.Services;

namespace TuneBox.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly SongService _songService;

        public SongsController(SongService songService)
        {
            _songService = songService;
        }

        // GET
        [HttpGet]
        public ActionResult<IEnumerable<Song>> GetAllSongs()
        {
            return Ok(_songService.GetAllSongs());
        }

        // GET by ID
        [HttpGet("{id}")]
        public ActionResult<Song> GetSongById(int id)
        {
            var song = _songService.GetSongById(id);

            if (song == null) return NotFound($"Song with ID {id} not found.");
            return Ok(song);
        }

        // POST
        [HttpPost]
        public ActionResult<Song> CreateSong([FromBody] Song newSong)
        {
            var result = _songService.CreateSong(newSong);

            if (result.ErrorMessage != null)
            {
                return BadRequest(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetSongById), new { id = result.Song!.Id }, result.Song);
        }

        // PUT
        [HttpPut("{id}")]
        public IActionResult UpdateSong(int id, [FromBody] Song updatedSong)
        {
            var result = _songService.UpdateSong(id, updatedSong);

            if (result.IsNotFound) return NotFound($"Song with ID {id} not found.");

            if (result.ErrorMessage != null)
            {
                return BadRequest(result.ErrorMessage);
            }

            return NoContent();
        }

        // DELETE by ID
        [HttpDelete("{id}")]
        public IActionResult DeleteSong(int id)
        {
            var isDeleted = _songService.DeleteSong(id);

            if (!isDeleted) return NotFound($"Song with ID {id} not found.");

            return NoContent();
        }

        // DELETE
        [HttpDelete]
        public IActionResult DeleteAllSongs()
        {
            _songService.DeleteAllSongs();
            return NoContent();
        }
    }
}