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
    public class GenreController : ControllerBase
    {
        private readonly GenreService _genreService;

        private readonly ILogger<GenreController> libraryLog;

        public GenreController(ILogger<GenreController> logger, GenreService genreService)
        {
            libraryLog = logger;
            _genreService = genreService;
        }

        [HttpGet("SearchGenre")]
        public async Task<ActionResult<JObject>> SearchGenre([FromQuery]string name = "", [FromQuery] string id = "")
        {
            JObject returnObject;
            List<Genre> genres = null;
            
            if(String.IsNullOrEmpty(name) && String.IsNullOrEmpty(id))
            {
                genres = await _genreService.ShowGenres();
            } 
            else
            {
                if(String.IsNullOrEmpty(name))
                {
                    genres = await _genreService.SearchGenreById(id);
                }
                else
                {
                    genres = await _genreService.SearchGenreByName(name);
                }
            }

            if (genres.Count < 1)
            {
                string errorMessage  = "No genre was found";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }

            returnObject = JSONFormatter.SuccessMessageFormatter("Genre found", genres);
            return Ok(returnObject);
        }

        [HttpPost("UploadGenre")]
        public async Task<ActionResult<Genre>> UploadGenre([FromBody] Genre newGenre)
        {
            JObject returnObject;

            if (newGenre == null)
            {
                libraryLog.LogError("REGISTER GENRE ERROR: Fields cannot be empty");
                string errorMessage = "The fields for the new genre are empty. Please fill all fields and try again";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }
            else
            {
                Genre proxyGenre = null;

                try
                {
                    bool isIdDuplicate = true;
                    string id = Guid.NewGuid().ToString();
                    newGenre.Id = id;

                    do
                    {
                        proxyGenre = await _genreService.GetGenre(newGenre.Id);

                        if (proxyGenre == null)
                        {
                            isIdDuplicate = false;
                        }
                        else
                        {
                            newGenre.Id = Guid.NewGuid().ToString();
                        }

                    } while (isIdDuplicate);

                    int counter = (_genreService.ShowGenres().Result.Count + 1);
                    newGenre.Id = counter.ToString();
                    proxyGenre = await _genreService.AddNewGenre(newGenre);
                    libraryLog.LogInformation("REGISTER GENTRE SUCCESSFUL: {0}", proxyGenre.Name);
                    returnObject = JSONFormatter.SuccessMessageFormatter("Genre registered successfully", proxyGenre);
                    return Ok(returnObject);
                }
                catch (Exception ex)
                {
                    libraryLog.LogError("REGISTER GENRE EXCEPTION:\n" + ex.Message);
                    returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                    return BadRequest(returnObject);
                }
            }
        }

        [HttpPut("UpdateGenre")]
        public async Task<ActionResult<JObject>> UpdateGenre([FromBody]Genre update)
        {
            JObject returnObject;
            Genre selectedGenre = null;

            try
            {
                selectedGenre = await _genreService.GetGenre(update.Id);

                if (selectedGenre == null)
                {
                    libraryLog.LogError("UPDATE GENRE ERROR: Genre not found");
                    string errorMessage = "Genre not found";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                else
                {
                    Genre proxyGenre = null;
                    selectedGenre.Name = update.Name;
                    proxyGenre = await _genreService.UpdateGenre(selectedGenre);
                    libraryLog.LogInformation("UPDATE GENRE SUCCESSFUL: {0}", proxyGenre.Name);
                    returnObject = JSONFormatter.SuccessMessageFormatter("Genre updated successfully", proxyGenre);
                    return Ok(returnObject);
                }
            }
            catch (Exception ex)
            {
                libraryLog.LogError("UPDATE GENRE EXCEPTION:\n" + ex.Message);
                returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                return BadRequest(returnObject);
            }
        }

        [HttpDelete("DeleteGenre")]
        public async Task<ActionResult<JObject>> DeleteGenre([FromQuery]string id)
        {
            JObject returnObject;
            Genre selectedGenre = null;

            try
            {
                selectedGenre = await _genreService.GetGenre(id);

                if (selectedGenre == null)
                {
                    libraryLog.LogError("DELETE GENRE ERROR: Genre not found");
                    string errorMessage = "Genre not found";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                else
                {
                    bool isDeleted = false;
                    isDeleted = await _genreService.DeleteGenre(id);

                    if(isDeleted)
                    {
                        libraryLog.LogInformation("DELETE GENRE SUCCESSFUL: {0}", selectedGenre.Name);
                        returnObject = JSONFormatter.SuccessMessageFormatter("Genre deleted successfully", selectedGenre);
                        return Ok(returnObject);
                    }
                    else
                    {
                        libraryLog.LogError("DELETE GENRE ERROR: Could not delete genre");
                        string errorMessage = "Could not delete genre due to a connection error.";
                        returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                        return BadRequest(returnObject);
                    }
                }
            }
            catch (Exception ex)
            {
                libraryLog.LogError("DELETE GENRE EXCEPTION:\n" + ex.Message);
                returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                return BadRequest(returnObject);
            }
        }
    }
}