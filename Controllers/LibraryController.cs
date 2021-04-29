/*
 Date: 17/04/2021
 Author(s): Ricardo Moguel Sanchez
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MSPublicLibrary.Models;

namespace MSPublicLibrary.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private LibrariesContext library_dbContext;
        private readonly ILogger<LibraryController> libraryLog;

        public LibraryController(ILogger<LibraryController> logger) 
        {
            libraryLog = logger;
            library_dbContext = new LibrariesContext();
        }

        [HttpGet("SearchSong")]
        public async Task<ActionResult<Song>> SearchSong([FromQuery]string title = "", [FromQuery] string songId = "")
        {
            List<Song> querySongs = null;

            querySongs = await library_dbContext.Songs
                .Where(song => song.Title.Contains(title))
                .Where(song => (!String.IsNullOrEmpty(songId) && song.SongId.Equals(songId)) || 
                (String.IsNullOrEmpty(songId) && !song.SongId.Equals(songId)))
                .ToListAsync();

            if (querySongs == null)
            {
                return BadRequest();
            }

            return Ok(querySongs);
        }

        [HttpPost("UploadSong")]
        public async Task<ActionResult<Song>> UploadSong([FromBody]Song newSong)
        {
            if(newSong == null)
            {
                libraryLog.LogError("REGISTER SONG ERROR: Fields cannot be empty");
                return BadRequest("The fields for the new song are empty. Please fill all fields and try again");
            }
            else
            {
                Song proxySong = null;

                try
                {
                    bool isIdDuplicate = true;
                    string id = Guid.NewGuid().ToString();
                    newSong.SongId = id;

                    do
                    {
                        proxySong = await library_dbContext.Songs
                        .Where(duplicate => duplicate.SongId.Contains(newSong.SongId)).FirstOrDefaultAsync();

                        if (proxySong == null)
                        {
                            isIdDuplicate = false;
                        }
                        else
                        {
                            newSong.SongId = Guid.NewGuid().ToString();
                        }

                    } while (isIdDuplicate);

                    library_dbContext.Entry(newSong).State = EntityState.Added;
                    library_dbContext.AddRange(newSong.Album);
                    await library_dbContext.SaveChangesAsync();
                    libraryLog.LogInformation("REGISTER SONG SUCCESSFUL: {0}", newSong.Title);
                    return Created("", newSong);
                }
                catch (Exception ex)
                {
                    libraryLog.LogError("REGISTER EXCEPTION:\n" + ex.Message);
                    return BadRequest(ex);
                }
            }      
        }

        [HttpPut("UpdateSong")]
        public async Task<ActionResult<Song>> UpdateSong([FromBody]Song song)
        {
            if(song == null)
            {
                libraryLog.LogError("UPDATE ERROR: The song was not found");
                return BadRequest("ERROR: The song to update could not be found.");
            }

            try
            {
                var selectedSong = library_dbContext.Songs.SingleOrDefault(s => s.SongId.Equals(song.SongId));
                selectedSong = song;
                library_dbContext.Entry(selectedSong).State = EntityState.Modified;
                await library_dbContext.SaveChangesAsync();
                
                libraryLog.LogInformation("UPDATE SONG SUCCESSFUL: {0}", selectedSong.Title);
                return Ok(selectedSong);
            }
            catch (Exception ex)
            {
                libraryLog.LogError("UPDATE EXCEPTION:\n" + ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet("QueryGenreSongs")]
        public async Task<ActionResult<Song>> QueryGenreSongs([FromBody]string genreID)
        {
            List<Song> querySongs = null;

            querySongs = await library_dbContext.Songs
                .Where(song => song.GenreId.Equals(genreID))
                .ToListAsync();

            if (querySongs == null)
            {
                return BadRequest("ERROR: No se encuentra ninguna canción con el genero seleccionado");
            }

            return Ok(querySongs);
        }

        [HttpGet("QueryArtistSongs")]
        public async Task<ActionResult<Song>> QueryArtistSongs([FromBody]string artistID)
        {
            List<Song> querySongs = null;

            querySongs = await library_dbContext.Songs
                .Where(song => song.ArtistId.Contains(artistID))
                .ToListAsync();

            if (querySongs == null)
            {
                return BadRequest("ERROR: No se encuentra ninguna canción con el artista seleccionado");
            }

            return Ok(querySongs);
        }

        [HttpGet("QueryAlbumSongs")]
        public async Task<ActionResult<Song>> QueryAlbumSongs([FromBody]string albumID)
        {
            List<Song> querySongs = null;

            querySongs = await library_dbContext.Songs
                .Where(song => song.ArtistId.Contains(albumID))
                .ToListAsync();

            if (querySongs == null)
            {
                return BadRequest("ERROR: No se encuentra ninguna canción con el artista seleccionado");
            }

            return Ok(querySongs);
        }
    }
}