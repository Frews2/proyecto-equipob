/*
 Date: 11/05/2021
 Author(s): Ricardo Moguel Sanchez
*/
using System;
using MSPrivateLibrary.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSPrivateLibrary.Services
{
    public class SongStatusService
    {
        private readonly IMongoCollection<SongStatus> status;

        public SongStatusService(IPrivateLibraryDatabaseSettings settings)
        {
            var client = new MongoClient(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            var database = client.GetDatabase(settings.DatabaseName);
            
            status = database.GetCollection<SongStatus>(settings.SongStatusCollectionName);
        }

        public async Task<List<SongStatus>> ShowSongStatuses()
        {
            List<SongStatus> statuses = await status.Find(p => true).ToListAsync();
            return statuses;
        }

        public async Task<SongStatus> GetStatus(string id)
        {
            SongStatus query = await status.Find<SongStatus>(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            return query;
        }
    }
}