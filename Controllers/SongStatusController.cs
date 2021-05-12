/*
 Date: 11/05/2021
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
    public class SongStatusController : ControllerBase
    {
        private readonly SongStatusService _statusService;

        private readonly ILogger<SongStatusController> libraryLog;

        public SongStatusController(ILogger<SongStatusController> logger, SongStatusService statusService)
        {
            libraryLog = logger;
            _statusService = statusService;
        }

        [HttpGet("ShowStatuses")]
        public async Task<ActionResult<JObject>> ShowStatuses()
        {
            JObject returnObject;
            List<SongStatus> status = null;
            
            status = await _statusService.ShowSongStatuses();

            if (status.Count < 1)
            {
                string errorMessage  = "No status was found";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }

            returnObject = JSONFormatter.SuccessMessageFormatter("Statuses found", status);
            return Ok(returnObject);
        }
    }
}