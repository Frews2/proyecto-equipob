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
using MSPublicLibrary.Models;
using MSPublicLibrary.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MongoDB.Driver;
using MSPublicLibrary.Utilities;

namespace MSPublicLibrary.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
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

            if (songs == null)
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
            if (newSong == null)
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

                    proxySong = await _songService.AddNewSong(newSong);
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

        [HttpGet("QueryGenreSongs")]
        public async Task<ActionResult<Song>> QueryGenreSongs([FromBody] string genreID)
        {
            JObject returnObject;
            List<Song> querySongs = null;

            querySongs = await _songService.SearchSongByGenre(genreID);

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
        public async Task<ActionResult<Song>> QueryArtistSongs([FromBody] string artistID)
        {
            JObject returnObject;
            List<Song> querySongs = null;

            querySongs = await _songService.SearchSongByArtist(artistID);

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
        public async Task<ActionResult<Song>> QueryAlbumSongs([FromBody] string albumID)
        {
            JObject returnObject;
            List<Song> querySongs = null;

            querySongs = await _songService.SearchSongByAlbum(albumID);

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
    }
}