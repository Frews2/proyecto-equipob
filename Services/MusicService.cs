/*
 Date: 09/05/2021
 Author(s): Ricardo Moguel Sanchez
*/
using System;
using MSPrivateLibrary.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPrivateLibrary.Services
{
    public class MusicService
    {
        private readonly IMongoCollection<Music> music;

        public MusicService(IPrivateLibraryDatabaseSettings settings)
        {
            var client = new MongoClient(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            var database = client.GetDatabase(settings.DatabaseName);
            
            music = database.GetCollection<Music>(settings.MusicCollectionName);
        }

        public async Task<List<Music>> ShowMusic()
        {
            List<Music> musicList = await music.Find(p => true).ToListAsync();
            return musicList;
        }

        public async Task<List<Music>> SearchMusicById(string id)
        {
            List<Music> query = await music.Find<Music>(x => x.Id.Equals(id)).ToListAsync();
            return query;
        }

        public async Task<List<Music>> SearchMusicByAddress(string address)
        {
            List<Music> query = await music.Find<Music>(x => x.Address.Contains(address)).ToListAsync();
            return query;
        }

        public async Task<Music> GetMusic(string id)
        {
            Music query = await music.Find<Music>(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            return query;
        }

        public async Task<Music> AddNewMusic(Music newMusic)
        {
            await music.InsertOneAsync(newMusic);
            return newMusic;
        }

        public async Task<Music> UpdateMusic(Music update)
        {
            await music.ReplaceOneAsync(query => query.Id.Equals(update.Id), update);
            return update;
        }

        public async Task<bool> DeleteMusic(string id)
        {
            await music.DeleteOneAsync(query => query.Id.Equals(id));
            return true;
        }
    }
}