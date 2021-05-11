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
    public class GenreService
    {
        private readonly IMongoCollection<Genre> genre;

        public GenreService(IPublicLibraryDatabaseSettings settings)
        {
            var client = new MongoClient(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            var database = client.GetDatabase(settings.DatabaseName);
            
            genre = database.GetCollection<Genre>(settings.GenreCollectionName);
        }

        public async Task<List<Genre>> ShowGenres()
        {
            List<Genre> genres = await genre.Find(p => true).ToListAsync();
            return genres;
        }

        public async Task<Genre> GetGenre(string id)
        {
            Genre query = await genre.Find<Genre>(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            return query;
        }

        public async Task<List<Genre>> SearchGenreById(string id)
        {
            List<Genre> query = await genre.Find<Genre>(x => x.Id.Equals(id)).ToListAsync();
            return query;
        }

        public async Task<List<Genre>> SearchGenreByName(string name)
        {
            List<Genre> query = await genre.Find<Genre>(x => x.Name.Contains(name)).ToListAsync();
            return query;
        }

        public async Task<Genre> AddNewGenre(Genre newGenre)
        {
            await genre.InsertOneAsync(newGenre);
            return newGenre;
        }
    }
}