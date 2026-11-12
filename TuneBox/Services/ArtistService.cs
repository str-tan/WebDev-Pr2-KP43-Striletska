using TuneBox.Models;

namespace TuneBox.Services
{
    public class ArtistService
    {
        private readonly DataService _dataService;

        public ArtistService(DataService dataService)
        {
            _dataService = dataService;
        }

        public IEnumerable<Artist> GetAllArtists()
        {
            var data = _dataService.LoadData();
            return data.Artists;
        }

        public Artist? GetArtistById(int id)
        {
            var data = _dataService.LoadData();
            return data.Artists.FirstOrDefault(a => a.Id == id);
        }

        public Artist CreateArtist(Artist newArtist)
        {
            var data = _dataService.LoadData();
            int newId = data.Artists.Any() ? data.Artists.Max(a => a.Id) + 1 : 1;
            newArtist.Id = newId;
            data.Artists.Add(newArtist);
            _dataService.SaveData(data);
            return newArtist;
        }

        public bool UpdateArtist(int id, Artist updatedArtist)
        {
            var data = _dataService.LoadData();
            var existingArtist = data.Artists.FirstOrDefault(a => a.Id == id);

            if (existingArtist == null) return false;

            existingArtist.Name = updatedArtist.Name;
            existingArtist.Country = updatedArtist.Country;
            _dataService.SaveData(data);
            return true;
        }

        public bool DeleteArtist(int id)
        {
            var data = _dataService.LoadData();
            var artistToRemove = data.Artists.FirstOrDefault(a => a.Id == id);

            if (artistToRemove == null) return false;

            data.Artists.Remove(artistToRemove);

            //Cascade delete
            data.Songs.RemoveAll(s => s.ArtistId == id);

            _dataService.SaveData(data);
            return true;
        }

        public void DeleteAllArtists()
        {
            var data = _dataService.LoadData();
            data.Artists.Clear();
            data.Songs.Clear();
            _dataService.SaveData(data);
        }

        public IEnumerable<Song>? GetSongsByArtist(int id)
        {
            var data = _dataService.LoadData();
            if (!data.Artists.Any(a => a.Id == id)) return null;

            return data.Songs.Where(s => s.ArtistId == id).ToList();
        }
    }
}