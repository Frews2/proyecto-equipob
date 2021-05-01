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
    public class PasswordController : ControllerBase
    {
        AccountsContext password_dbContext;

        private readonly ILogger<PasswordController> passwordLogger;

        public PasswordController(ILogger<PasswordController> logger)
        {
            passwordLogger = logger;
            password_dbContext = new AccountsContext();
        }

        [HttpPut("UpdatePassword")]
        public async Task<ActionResult<JObject>> UpdatePassword([FromBody] Password password)
        {
            JObject returnObject;

            if(password == null)
            {
                passwordLogger.LogError("UPDATE ERROR: The password was not found");
                string errorMessage = "ERROR: The password to update could not be found.";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }

            Password selectedPassword = null;

            try
            {
                selectedPassword = await password_dbContext.Passwords
                .Where(p => p.PasswordId.Contains(password.PasswordId) && p.OwnerId.Equals(password.OwnerId))
                .FirstOrDefaultAsync();

                if(selectedPassword != null)
                {
                    selectedPassword.PasswordString = password.PasswordString;
                    password_dbContext.Entry(selectedPassword).State = EntityState.Modified;
                    await password_dbContext.SaveChangesAsync();
                
                    passwordLogger.LogInformation("UPDATE PASSWORD SUCCESSFUL: {0}", password.PasswordId);
                    returnObject = JSONFormatter.SuccessMessageFormatter("Password update successful", selectedPassword);
                    return Ok(returnObject);
                }
                else
                {
                    string errorMessage  = "Could not locate password to update";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                
            }
            catch (Exception ex)
            {
                passwordLogger.LogError("UPDATE EXCEPTION:\n" + ex.Message);
                returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                return BadRequest(returnObject);
            }
        }
    }
}