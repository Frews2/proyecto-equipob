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
    public class AlbumController : ControllerBase
    {
        private readonly AlbumService _albumService;

        private readonly ILogger<AlbumController> libraryLog;

        public AlbumController(ILogger<AlbumController> logger, AlbumService albumService)
        {
            libraryLog = logger;
            _albumService = albumService;
        }

        [HttpGet("SearchAlbum")]
        public async Task<ActionResult<JObject>> SearchAlbum([FromQuery]string name = "", [FromQuery] string id = "")
        {
            JObject returnObject;
            List<Album> albums = null;
            
            if(String.IsNullOrEmpty(name) && String.IsNullOrEmpty(id))
            {
                albums = await _albumService.ShowAlbums();
            } 
            else
            {
                if(String.IsNullOrEmpty(name))
                {
                    albums = await _albumService.SearchAlbumById(id);
                }
                else
                {
                    albums = await _albumService.SearchAlbumByName(name);
                }
            }

            if (albums.Count < 1)
            {
                string errorMessage  = "No album was found";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }

            returnObject = JSONFormatter.SuccessMessageFormatter("Album found", albums);
            return Ok(returnObject);
        }

        [HttpPost("UploadAlbum")]
        public async Task<ActionResult<Album>> UploadAlbum([FromBody] Album newAlbum)
        {
            JObject returnObject;

            if (newAlbum == null)
            {
                libraryLog.LogError("REGISTER ALBUM ERROR: Fields cannot be empty");
                string errorMessage = "The fields for the new album are empty. Please fill all fields and try again";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }
            else
            {
                Album proxyAlbum = null;

                try
                {
                    bool isIdDuplicate = true;
                    string id = Guid.NewGuid().ToString();
                    newAlbum.Id = id;

                    do
                    {
                        proxyAlbum = await _albumService.GetAlbum(newAlbum.Id);

                        if (proxyAlbum == null)
                        {
                            isIdDuplicate = false;
                        }
                        else
                        {
                            newAlbum.Id = Guid.NewGuid().ToString();
                        }

                    } while (isIdDuplicate);

                    proxyAlbum = await _albumService.AddNewAlbum(newAlbum);
                    libraryLog.LogInformation("REGISTER ALBUM SUCCESSFUL: {0}", proxyAlbum.Name);
                    returnObject = JSONFormatter.SuccessMessageFormatter("Album registered successfully", proxyAlbum);
                    return Ok(returnObject);
                }
                catch (Exception ex)
                {
                    libraryLog.LogError("ALBUM REGISTER EXCEPTION:\n" + ex.Message);
                    returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                    return BadRequest(returnObject);
                }
            }
        }

        [HttpPut("UpdateAlbum")]
        public async Task<ActionResult<JObject>> UpdateAlbum([FromBody]Album update)
        {
            JObject returnObject;
            Album selectedAlbum = null;

            try
            {
                selectedAlbum = await _albumService.GetAlbum(update.Id);

                if (selectedAlbum == null)
                {
                    libraryLog.LogError("UPDATE ALBUM ERROR: Album not found");
                    string errorMessage = "Album not found";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                else
                {
                    Album proxyAlbum = null;
                    selectedAlbum.Name = update.Name;
                    selectedAlbum.ImageAddress = update.ImageAddress;
                    selectedAlbum.ImageName = update.ImageName;
                    selectedAlbum.ImageType = update.ImageType;
                    proxyAlbum = await _albumService.UpdateAlbum(selectedAlbum);
                    libraryLog.LogInformation("UPDATE ALBUM SUCCESSFUL: {0}", proxyAlbum.Name);
                    returnObject = JSONFormatter.SuccessMessageFormatter("Album approved successfully", proxyAlbum);
                    return Ok(returnObject);
                }
            }
            catch (Exception ex)
            {
                libraryLog.LogError("UPDATE ALBUM EXCEPTION:\n" + ex.Message);
                returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                return BadRequest(returnObject);
            }
        }

        [HttpDelete("DeleteAlbum")]
        public async Task<ActionResult<JObject>> DeleteAlbum([FromQuery]string id)
        {
            JObject returnObject;
            Album selectedAlbum = null;

            try
            {
                selectedAlbum = await _albumService.GetAlbum(id);

                if (selectedAlbum == null)
                {
                    libraryLog.LogError("DELETE ALBUM ERROR: Album not found");
                    string errorMessage = "Album not found";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                else
                {
                    bool isDeleted = false;
                    isDeleted = await _albumService.DeleteAlbum(id);

                    if(isDeleted)
                    {
                        libraryLog.LogInformation("DELETE ALBUM SUCCESSFUL: {0}", selectedAlbum.Name);
                        returnObject = JSONFormatter.SuccessMessageFormatter("Album deleted successfully", selectedAlbum);
                        return Ok(returnObject);
                    }
                    else
                    {
                        libraryLog.LogError("DELETE ALBUM ERROR: Could not delete album");
                        string errorMessage = "Could not delete album due to a connection error.";
                        returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                        return BadRequest(returnObject);
                    }
                }
            }
            catch (Exception ex)
            {
                libraryLog.LogError("DELETE ALBUM EXCEPTION:\n" + ex.Message);
                returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                return BadRequest(returnObject);
            }
        }
    }
}