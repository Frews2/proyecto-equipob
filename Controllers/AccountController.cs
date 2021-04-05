using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MSCuenta.Models;

namespace MSCuenta.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private AccountsContext dbContext;
        private readonly ILogger<AccountController> log;

        public AccountController(ILogger<AccountController> logger) 
        {
            log = logger;
            dbContext = new AccountsContext();
        }

        [HttpGet("search")]
        public async Task<ActionResult<Account>> Search([FromQuery]string username = "", [FromQuery] int id = -1)
        {
            List<Account> accounts = null;

            accounts = await dbContext.Accounts
                .Where(account => account.Username.Contains(username))
                .Where(account => (id >= 0 && account.AccountId == id) || 
                (id < 0 && account.AccountId != id))
                .ToListAsync();

            if (accounts == null)
            {
                return BadRequest();
            }

            return Ok(accounts);
        }

        [HttpPost("RegisterAccount")]
        public async Task<ActionResult<Account>> Add([FromBody]Account newAccount)
        {
            if(newAccount == null)
            {
                log.LogError("ERROR: Fields cannot be empty");
                return BadRequest("The fields for the new account are empty. Please fill all fields and try again");
            }

            try
            {
                dbContext.Entry(newAccount).State = EntityState.Added;
                await dbContext.SaveChangesAsync();

                log.LogInformation("REGISTER ACCOUNT SUCCESSFUL: {0}", newAccount.Username);
                return Created("", newAccount);
            }
            catch (Exception ex)
            {
                log.LogError("REGISTER EXCEPTION:\n" + ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPut("UpdateAccount")]
        public async Task<ActionResult<Account>> update([FromBody]Account account)
        {
            if(account == null)
            {
                log.LogError("UPDATE ERROR: The account was not found");
                return BadRequest("ERROR: The account to update could not be found.");
            }

            try
            {
                var selectedAccount = dbContext.Accounts.SingleOrDefault(a => a.AccountId == account.AccountId);
                selectedAccount = account;
                await dbContext.SaveChangesAsync();
                
                log.LogInformation("UPDATE ACCOUNT SUCCESSFUL: {0}", account.Username);
                return Ok(account);
            }
            catch (Exception ex)
            {
                log.LogError("UPDATE EXCEPTION:\n" + ex.Message);
                return BadRequest(ex);
            }
        }
    }
}