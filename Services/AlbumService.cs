/*
 Date: 08/05/2021
 Author(s): Ricardo Moguel Sanchez
*/
using System;
using MSPublicLibrary.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSPublicLibrary.Services
{
    public class AlbumService
    {
        private readonly IMongoCollection<Album> album;

        public AlbumService(IPublicLibraryDatabaseSettings settings)
        {
            var client = new MongoClient(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            var database = client.GetDatabase(settings.DatabaseName);
            
            album = database.GetCollection<Album>(settings.AlbumCollectionName);
        }

        public async Task<List<Album>> ShowAlbums()
        {
            List<Album> albums = await album.Find(p => true).ToListAsync();
            return albums;
        }

        public async Task<Album> GetAlbum(string id)
        {
            Album query = await album.Find<Album>(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            return query;
        }

        public async Task<List<Album>> SearchAlbumById(string id)
        {
            List<Album> query = await album.Find<Album>(x => x.Id.Equals(id)).ToListAsync();
            return query;
        }

        public async Task<List<Album>> SearchAlbumByName(string name)
        {
            List<Album> query = await album.Find<Album>(x => x.Name.Contains(name)).ToListAsync();
            return query;
        }
        
        public async Task<Album> AddNewAlbum(Album newAlbum)
        {
            await album.InsertOneAsync(newAlbum);
            return newAlbum;
        }
    }
}