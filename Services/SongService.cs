/*
 Date: 09/05/2021
 Author(s): Ricardo Moguel Sanchez
*/
using System;
using MSPublicLibrary.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPublicLibrary.Services
{
    public class SongService
    {
        private readonly IMongoCollection<Song> song;

        public SongService(IPublicLibraryDatabaseSettings settings)
        {
            var client = new MongoClient(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            var database = client.GetDatabase(settings.DatabaseName);
            song = database.GetCollection<Song>(settings.SongCollectionName);
        }

        public async Task<List<Song>> ShowSongs()
        {
            List<Song> songs = await song.Find(p => true).ToListAsync();
            return songs;
        }

        public async Task<Song> GetSong(string id)
        {
            Song query = await song.Find<Song>(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            return query;
        }
        
        public async Task<List<Song>> SearchSongById(string songId)
        {
            List<Song> query = await song.Find<Song>(s => s.Id.Equals(songId)).ToListAsync();
            return query;
        }
            
        public async Task<List<Song>> SearchSongByTitle(string title)
        {
            List<Song> query = await song.Find<Song>(s => s.Title.Contains(title)).ToListAsync();
            return query;
        }

        public async Task<List<Song>> SearchSongByAlbum(string albumId)
        {
            List<Song> query = await song.Find<Song>(s => s.AlbumId.Contains(albumId)).ToListAsync();
            return query;
        }

        public async Task<List<Song>> SearchSongByGenre(string genreId)
        {
            List<Song> query = await song.Find<Song>(s => s.GenreId.Contains(genreId)).ToListAsync();
            return query;
        }

        public async Task<List<Song>> SearchSongByArtist(string artistId)
        {
            List<Song> query = await song.Find<Song>(s => s.ArtistId.Contains(artistId)).ToListAsync();
            return query;
        }

        public async Task<Song> AddNewSong(Song newSong)
        {
            await song.InsertOneAsync(newSong);
            return newSong;
        }
    }
}