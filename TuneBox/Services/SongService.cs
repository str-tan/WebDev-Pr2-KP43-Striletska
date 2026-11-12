using TuneBox.Models;

namespace TuneBox.Services
{
    public class SongService
    {
        private readonly DataService _dataService;

        public SongService(DataService dataService)
        {
            _dataService = dataService;
        }

        public IEnumerable<Song> GetAllSongs()
        {
            var data = _dataService.LoadData();
            return data.Songs;
        }

        public Song? GetSongById(int id)
        {
            var data = _dataService.LoadData();
            return data.Songs.FirstOrDefault(s => s.Id == id);
        }

        public (Song? Song, string? ErrorMessage) CreateSong(Song newSong)
        {
            var data = _dataService.LoadData();
            if (!data.Artists.Any(a => a.Id == newSong.ArtistId))
            {
                return (null, $"Artist with ID {newSong.ArtistId} does not exist.");
            }

            int newId = data.Songs.Any() ? data.Songs.Max(s => s.Id) + 1 : 1;
            newSong.Id = newId;
            data.Songs.Add(newSong);
            _dataService.SaveData(data);
            return (newSong, null);
        }

        public (bool IsSuccess, bool IsNotFound, string? ErrorMessage) UpdateSong(int id, Song updatedSong)
        {
            var data = _dataService.LoadData();
            var existingSong = data.Songs.FirstOrDefault(s => s.Id == id);

            if (existingSong == null) return (false, true, null);

            if (!data.Artists.Any(a => a.Id == updatedSong.ArtistId))
            {
                return (false, false, $"Artist with ID {updatedSong.ArtistId} does not exist.");
            }

            existingSong.Title = updatedSong.Title;
            existingSong.Duration = updatedSong.Duration;
            existingSong.Genre = updatedSong.Genre;
            existingSong.ArtistId = updatedSong.ArtistId;

            _dataService.SaveData(data);
            return (true, false, null);
        }

        public bool DeleteSong(int id)
        {
            var data = _dataService.LoadData();
            var songToRemove = data.Songs.FirstOrDefault(s => s.Id == id);

            if (songToRemove == null) return false;

            data.Songs.Remove(songToRemove);
            _dataService.SaveData(data);
            return true;
        }

        public void DeleteAllSongs()
        {
            var data = _dataService.LoadData();
            data.Songs.Clear();
            _dataService.SaveData(data);
        }
    }
}