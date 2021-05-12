/*
 Date: 10/05/2021
 Author(s): Ricardo Moguel Sanchez
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSPrivateLibrary.Models;
using MSPrivateLibrary.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MongoDB.Driver;
using MSPrivateLibrary.Utilities;

namespace MSPrivateLibrary.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private const string UNREQUESTED = "1";
        private const string REQUESTED = "2";
        private readonly SongService _songService;

        private readonly ILogger<SongController> libraryLog;

        public SongController(ILogger<SongController> logger, SongService songService)
        {
            libraryLog = logger;
            _songService = songService;
        }

        [HttpGet("SearchSong")]
        public async Task<ActionResult<JObject>> SearchSong([FromQuery]string title = "", [FromQuery] string id = "")
        {
            JObject returnObject;
            List<Song> songs = null;
            
            if(String.IsNullOrEmpty(title) && String.IsNullOrEmpty(id))
            {
                songs = await _songService.ShowSongs();
            } 
            else
            {
                if(String.IsNullOrEmpty(title))
                {
                    songs = await _songService.SearchSongById(id);
                }
                else
                {
                    songs = await _songService.SearchSongByTitle(title);
                }
            }

            if (songs.Count < 1)
            {
                string errorMessage  = "No song was found";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }

            returnObject = JSONFormatter.SuccessMessageFormatter("Song found", songs);
            return Ok(returnObject);
        }

        [HttpPost("UploadSong")]
        public async Task<ActionResult<Song>> UploadSong([FromBody] Song newSong)
        {
            JObject returnObject;

            if (newSong == null)
            {
                libraryLog.LogError("REGISTER SONG ERROR: Fields cannot be empty");
                string errorMessage = "The fields for the new song are empty. Please fill all fields and try again";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }
            else
            {
                Song proxySong = null;

                try
                {
                    bool isIdDuplicate = true;
                    string id = Guid.NewGuid().ToString();
                    newSong.Id = id;

                    do
                    {
                        proxySong = await _songService.GetSong(newSong.Id);

                        if (proxySong == null)
                        {
                            isIdDuplicate = false;
                        }
                        else
                        {
                            newSong.Id = Guid.NewGuid().ToString();
                        }

                    } while (isIdDuplicate);
                    newSong.StatusId = UNREQUESTED;
                    proxySong = await _songService.AddNewSong(newSong);
                    libraryLog.LogInformation("REGISTER SONG SUCCESSFUL: {0}", newSong.Title);
                    returnObject = JSONFormatter.SuccessMessageFormatter("Song registered successfully", proxySong);
                    return Ok(returnObject);
                }
                catch (Exception ex)
                {
                    libraryLog.LogError("REGISTER SONG EXCEPTION:\n" + ex.Message);
                    returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                    return BadRequest(returnObject);
                }
            }
        }

        [HttpGet("QueryGenreSongs")]
        public async Task<ActionResult<Song>> QueryGenreSongs([FromQuery] string genreId)
        {
            JObject returnObject;
            List<Song> querySongs = null;

            querySongs = await _songService.SearchSongByGenre(genreId);

            if (querySongs.Count < 1)
            {
                string errorMessage  = "No song was found";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }
            else
            {
                returnObject = JSONFormatter.SuccessMessageFormatter("Songs found", querySongs);
                return Ok(returnObject);
            }
        }

        [HttpGet("QueryArtistSongs")]
        public async Task<ActionResult<Song>> QueryArtistSongs([FromQuery] string artistId)
        {
            JObject returnObject;
            List<Song> querySongs = null;

            querySongs = await _songService.SearchSongByArtist(artistId);

            if (querySongs.Count < 1)
            {
                string errorMessage  = "No song was found";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }
            else
            {
                returnObject = JSONFormatter.SuccessMessageFormatter("Songs found", querySongs);
                return Ok(returnObject);
            }
        }

        [HttpGet("QueryAlbumSongs")]
        public async Task<ActionResult<Song>> QueryAlbumSongs([FromQuery] string albumId)
        {
            JObject returnObject;
            List<Song> querySongs = null;

            querySongs = await _songService.SearchSongByAlbum(albumId);

            if (querySongs.Count < 1)
            {
                string errorMessage  = "No song was found";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }
            else
            {
                returnObject = JSONFormatter.SuccessMessageFormatter("Songs found", querySongs);
                return Ok(returnObject);
            }
        }

        [HttpPut("RequestSong")]
        public async Task<ActionResult<JObject>> RequestSong([FromQuery]string songId)
        {
            JObject returnObject;
            Song selectedSong = null;

            try
            {
                selectedSong = await _songService.GetSong(songId);

                if (selectedSong == null)
                {
                    libraryLog.LogError("SONG REQUEST ERROR: Song not found");
                    string errorMessage = "Song not found";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                else
                {
                    Song proxySong = null;

                    if(selectedSong.StatusId.Equals(REQUESTED))
                    {
                        libraryLog.LogError("SONG REQUEST ERROR: Song already requested");
                        string errorMessage = "ERROR: This song has already been requested.";
                        returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                        return BadRequest(returnObject);
                    }
                    else
                    {
                        selectedSong.StatusId = REQUESTED;
                        proxySong = await _songService.UpdateSong(selectedSong);
                        libraryLog.LogInformation("SONG REQUEST SUCCESSFUL: {0}", proxySong.Title);
                        returnObject = JSONFormatter.SuccessMessageFormatter("Song requested successfully", proxySong);
                        return Ok(returnObject);
                    }         
                }
            }
            catch (Exception ex)
            {
                libraryLog.LogError("REJECT SONG REQUEST EXCEPTION:\n" + ex.Message);
                returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                return BadRequest(returnObject);
            }
        }

        [HttpPut("UpdateSong")]
        public async Task<ActionResult<JObject>> UpdateMusic([FromBody]Song update)
        {
            JObject returnObject;
            Song selectedSong = null;

            try
            {
                selectedSong = await _songService.GetSong(update.Id);

                if (selectedSong == null)
                {
                    libraryLog.LogError("UPDATE SONG ERROR: Song not found");
                    string errorMessage = "Song not found";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                else
                {
                    Song proxySong = null;
                    selectedSong.Title = update.Title;
                    selectedSong.ArtistId = update.ArtistId;
                    selectedSong.Composer = update.Composer;
                    selectedSong.Producer = update.Producer;
                    selectedSong.Duration = update.Duration;
                    selectedSong.ReleaseYear = update.ReleaseYear;
                    selectedSong.AlbumId = update.AlbumId;
                    selectedSong.GenreId = update.GenreId;
                    proxySong = await _songService.UpdateSong(selectedSong);
                    libraryLog.LogInformation("UPDATE SONG SUCCESSFUL: {0}", proxySong.Title);
                    returnObject = JSONFormatter.SuccessMessageFormatter("Song updated successfully", proxySong);
                    return Ok(returnObject);
                }
            }
            catch (Exception ex)
            {
                libraryLog.LogError("UPDATE SONG EXCEPTION:\n" + ex.Message);
                returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                return BadRequest(returnObject);
            }
        }

        [HttpDelete("DeleteSong")]
        public async Task<ActionResult<JObject>> DeleteSong([FromQuery]string id)
        {
            JObject returnObject;
            Song selectedSong = null;

            try
            {
                selectedSong = await _songService.GetSong(id);

                if (selectedSong == null)
                {
                    libraryLog.LogError("DELETE SONG ERROR: Song not found");
                    string errorMessage = "Song not found";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                else
                {
                    bool isDeleted = false;
                    isDeleted = await _songService.DeleteSong(id);

                    if(isDeleted)
                    {
                        libraryLog.LogInformation("DELETE SONG SUCCESSFUL: {0}", selectedSong.Title);
                        returnObject = JSONFormatter.SuccessMessageFormatter("SONG deleted successfully", selectedSong);
                        return Ok(returnObject);
                    }
                    else
                    {
                        libraryLog.LogError("DELETE SONG ERROR: Could not delete song");
                        string errorMessage = "Could not delete song due to a connection error.";
                        returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                        return BadRequest(returnObject);
                    }
                }
            }
            catch (Exception ex)
            {
                libraryLog.LogError("DELETE SONG EXCEPTION:\n" + ex.Message);
                returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                return BadRequest(returnObject);
            }
        }
    }
}