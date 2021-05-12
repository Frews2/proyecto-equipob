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
        private const string APPROVED = "1";
        private const string REJECTED = "2";
        private const string PENDING = "3";
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
                songs = await _songService.ShowApprovedSongs();
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

        [HttpGet("ShowPendingSongs")]
        public async Task<ActionResult<JObject>> SearchRequests()
        {
            JObject returnObject;
            List<Song> songs = null;
            
            songs = await _songService.ShowSongRequests();

            if (songs.Count < 1)
            {
                string errorMessage  = "There are no pending songs";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }

            returnObject = JSONFormatter.SuccessMessageFormatter("Song found", songs);
            return Ok(returnObject);
        }

        [HttpGet("ShowRejectedSongs")]
        public async Task<ActionResult<JObject>> SearchPending()
        {
            JObject returnObject;
            List<Song> songs = null;
            
            songs = await _songService.ShowSongRejects();

            if (songs.Count < 1)
            {
                string errorMessage  = "There are no rejected songs";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }

            returnObject = JSONFormatter.SuccessMessageFormatter("Song found", songs);
            return Ok(returnObject);
        }

        [HttpPost("UploadSongRequest")]
        public async Task<ActionResult<Song>> UploadSong([FromBody] Song newSong)
        {
            JObject returnObject;

            if (newSong == null)
            {
                libraryLog.LogError("REGISTER SONG REQUEST ERROR: Fields cannot be empty");
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
                    newSong.StatusId = PENDING;
                    proxySong = await _songService.AddNewSongPetition(newSong);
                    libraryLog.LogInformation("REGISTER SONG REQUEST SUCCESSFUL: {0}", newSong.Title);
                    returnObject = JSONFormatter.SuccessMessageFormatter("Song request registered successfully", proxySong);
                    return Ok(returnObject);
                }
                catch (Exception ex)
                {
                    libraryLog.LogError("REGISTER SONG REQUEST EXCEPTION:\n" + ex.Message);
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

        [HttpPut("RejectSong")]
        public async Task<ActionResult<JObject>> RejectSong([FromQuery]string songId)
        {
            JObject returnObject;
            Song selectedSong = null;

            try
            {
                selectedSong = await _songService.GetSong(songId);

                if (selectedSong == null)
                {
                    libraryLog.LogError("REJECT SONG REQUEST ERROR: Song not found");
                    string errorMessage = "Song not found";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                else
                {
                    Song proxySong = null;

                    if(selectedSong.StatusId.Equals(REJECTED))
                    {
                        libraryLog.LogError("REJECT SONG REQUEST ERROR: Song Request already rejected");
                        string errorMessage = "ERROR: This song has already been rejected.";
                        returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                        return BadRequest(returnObject);
                    }
                    else
                    {
                        if(selectedSong.StatusId.Equals(APPROVED))
                        {
                            libraryLog.LogError("REJECT SONG REQUEST ERROR: Approved Song cannot be rejected");
                            string errorMessage = "ERROR: This song has  been approved and cannot be rejected.";
                            returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                            return BadRequest(returnObject);
                        }
                        else
                        {
                            selectedSong.StatusId = REJECTED;
                            proxySong = await _songService.UpdateSong(selectedSong);
                            libraryLog.LogInformation("REJECT SONG REQUEST SUCCESSFUL: {0}", proxySong.Title);
                            returnObject = JSONFormatter.SuccessMessageFormatter("Song rejected successfully", proxySong);
                            return Ok(returnObject);
                        }
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

        [HttpPut("ApproveSong")]
        public async Task<ActionResult<JObject>> ApproveSong([FromQuery]string songId)
        {
            JObject returnObject;
            Song selectedSong = null;

            try
            {
                selectedSong = await _songService.GetSong(songId);

                if (selectedSong == null)
                {
                    libraryLog.LogError("APPROVE SONG REQUEST ERROR: Song not found");
                    string errorMessage = "Song not found";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                else
                {
                    Song proxySong = null;

                    if(selectedSong.StatusId.Equals(APPROVED))
                    {
                        libraryLog.LogError("APPROVE SONG REQUEST ERROR: Song Request already approved");
                        string errorMessage = "ERROR: This song has already been approved.";
                        returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                        return BadRequest(returnObject);
                    }
                    else
                    {
                        if(selectedSong.StatusId.Equals(REJECTED))
                        {
                            libraryLog.LogError("APPROVE SONG REQUEST ERROR: Rejected Song cannot be approved");
                            string errorMessage = "ERROR: This song has  been rejected and cannot be approved.";
                            returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                            return BadRequest(returnObject);
                        }
                        else
                        {
                            selectedSong.StatusId = APPROVED;
                            proxySong = await _songService.UpdateSong(selectedSong);
                            libraryLog.LogInformation("APPROVE SONG REQUEST SUCCESSFUL: {0}", proxySong.Title);
                            returnObject = JSONFormatter.SuccessMessageFormatter("Song approved successfully", proxySong);
                            return Ok(returnObject);
                        }
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
    }
}