/*
 Date: 30/04/2021
 Author(s): Ricardo Moguel Sanchez
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MSCuenta.Models;
using MSCuenta.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MSCuenta.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class StatusController : ControllerBase
    {
        AccountsContext status_dbContext;

        private readonly ILogger<StatusController> statusLogger;

        public StatusController(ILogger<StatusController> logger)
        {
            statusLogger = logger;
            status_dbContext = new AccountsContext();
        }

        [HttpGet("statuses")]
        public async Task<ActionResult<JObject>> Get([FromQuery]int id = -1)
        {
            JObject returnObject;
            List<Status> statusList = null;
            statusList = await status_dbContext.Statuses
                .Where(status => (id >= 0 && status.StatusId == id) || (id == -1 && status.StatusId != id))
                .ToListAsync();
            if(statusList == null)
            {
                string errorMessage  = "No statuses were found";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }
            else
            {
                returnObject = JSONFormatter.SuccessMessageFormatter("Statuses found", statusList);
                return Ok(returnObject);
            }
        }
    }
}