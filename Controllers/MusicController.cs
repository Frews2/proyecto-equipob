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
    public class MusicController : ControllerBase
    {
        private readonly MusicService _musicService;
        private readonly ILogger<MusicController> libraryLog;

        public MusicController(ILogger<MusicController> logger, MusicService musicService)
        {
            libraryLog = logger;
            _musicService = musicService;
        }

        [HttpGet("SearchMusic")]
        public async Task<ActionResult<JObject>> SearchMusic([FromQuery]string address = "", [FromQuery] string id = "")
        {
            JObject returnObject;
            List<Music> music = null;
            
            if(String.IsNullOrEmpty(address) && String.IsNullOrEmpty(id))
            {
                music = await _musicService.ShowMusic();
            } 
            else
            {
                if(String.IsNullOrEmpty(address))
                {
                    music = await _musicService.SearchMusicById(id);
                }
                else
                {
                    music = await _musicService.SearchMusicByAddress(address);
                }
            }

            if (music.Count < 1)
            {
                string errorMessage  = "No music was found";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }

            returnObject = JSONFormatter.SuccessMessageFormatter("Music found", music);
            return Ok(returnObject);
        }

        [HttpPost("UploadMusic")]
        public async Task<ActionResult<Music>> UploadMusic([FromBody] Music newMusic)
        {
            JObject returnObject;

            if (newMusic == null)
            {
                libraryLog.LogError("REGISTER MUSIC ERROR: Fields cannot be empty");
                string errorMessage = "The fields for the new music are empty. Please fill all fields and try again";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }
            else
            {
                Music proxyMusic = null;

                try
                {
                    bool isIdDuplicate = true;
                    string id = Guid.NewGuid().ToString();
                    newMusic.Id = id;

                    do
                    {
                        proxyMusic = await _musicService.GetMusic(newMusic.Id);

                        if (proxyMusic == null)
                        {
                            isIdDuplicate = false;
                        }
                        else
                        {
                            newMusic.Id = Guid.NewGuid().ToString();
                        }

                    } while (isIdDuplicate);

                    proxyMusic = await _musicService.AddNewMusic(newMusic);
                    libraryLog.LogInformation("REGISTER MUSIC SUCCESSFUL: {0}", proxyMusic.Name);
                    returnObject = JSONFormatter.SuccessMessageFormatter("Music registered successfully", proxyMusic);
                    return Ok(returnObject);
                }
                catch (Exception ex)
                {
                    libraryLog.LogError("REGISTER MUSIC EXCEPTION:\n" + ex.Message);
                    returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                    return BadRequest(returnObject);
                }
            }
        }

        [HttpPut("UpdateMusic")]
        public async Task<ActionResult<JObject>> UpdateMusic([FromBody]Music update)
        {
            JObject returnObject;
            Music selectedMusic = null;

            try
            {
                selectedMusic = await _musicService.GetMusic(update.Id);

                if (selectedMusic == null)
                {
                    libraryLog.LogError("UPDATE MUSIC ERROR: Music not found");
                    string errorMessage = "Music not found";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                else
                {
                    Music proxyMusic = null;
                    proxyMusic = await _musicService.UpdateMusic(update);
                    libraryLog.LogInformation("UPDATE MUSIC SUCCESSFUL: {0}", proxyMusic.Name);
                    returnObject = JSONFormatter.SuccessMessageFormatter("Music updated successfully", proxyMusic);
                    return Ok(returnObject);
                }
            }
            catch (Exception ex)
            {
                libraryLog.LogError("UPDATE MUSIC EXCEPTION:\n" + ex.Message);
                returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                return BadRequest(returnObject);
            }
        }

        [HttpPut("DeleteMusic")]
        public async Task<ActionResult<JObject>> DeleteMusic([FromQuery]string id)
        {
            JObject returnObject;
            Music selectedMusic = null;

            try
            {
                selectedMusic = await _musicService.GetMusic(id);

                if (selectedMusic == null)
                {
                    libraryLog.LogError("DELETE MUSIC ERROR: Music not found");
                    string errorMessage = "Music not found";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                else
                {
                    bool isDeleted = false;
                    isDeleted = await _musicService.DeleteMusic(id);

                    if(isDeleted)
                    {
                        libraryLog.LogInformation("DELETE MUSIC SUCCESSFUL: {0}", selectedMusic.Name);
                        returnObject = JSONFormatter.SuccessMessageFormatter("Music deleted successfully", selectedMusic);
                        return Ok(returnObject);
                    }
                    else
                    {
                        libraryLog.LogError("DELETE MUSIC ERROR: Could not delete music");
                        string errorMessage = "Could not delete music due to a connection error.";
                        returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                        return BadRequest(returnObject);
                    }
                }
            }
            catch (Exception ex)
            {
                libraryLog.LogError("DELETE MUSIC EXCEPTION:\n" + ex.Message);
                returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                return BadRequest(returnObject);
            }
        }
    }
}