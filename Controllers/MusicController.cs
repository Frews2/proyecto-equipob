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
        private IMongoCollection<Music> music;
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

            if (music == null)
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
            if (newMusic == null)
            {
                libraryLog.LogError("REGISTER MUSIC ERROR: Fields cannot be empty");
                return BadRequest("The fields for the new music are empty. Please fill all fields and try again");
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
                    libraryLog.LogInformation("REGISTER ALBUM SUCCESSFUL: {0}", proxyMusic.Name);
                    return Created("", proxyMusic);
                }
                catch (Exception ex)
                {
                    libraryLog.LogError("REGISTER EXCEPTION:\n" + ex.Message);
                    return BadRequest(ex);
                }
            }
        }
    }
}