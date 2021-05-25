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
    public class MusicController : ControllerBase
    {
        private readonly MusicService _musicService;
        private readonly ILogger<MusicController> libraryLog;

        public MusicController(ILogger<MusicController> logger, MusicService musicService)
        {
            libraryLog = logger;
            _musicService = musicService;
        }

        [HttpPost("SearchMusic")]
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
    }
}