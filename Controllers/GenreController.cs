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

            if (genres == null)
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
            if (newGenre == null)
            {
                libraryLog.LogError("REGISTER GENRE ERROR: Fields cannot be empty");
                return BadRequest("The fields for the new genre are empty. Please fill all fields and try again");
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
                    return Created("", proxyGenre);
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