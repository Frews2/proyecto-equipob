/*
 Date: 17/04/2021
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

namespace MSCuenta.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private AccountsContext account_dbContext;
        private readonly ILogger<AccountController> accountLog;

        public AccountController(ILogger<AccountController> logger) 
        {
            accountLog = logger;
            account_dbContext = new AccountsContext();
        }

        [HttpGet("SearchAccount")]
        public async Task<ActionResult<Account>> SearchAccount([FromQuery]string username = "", [FromQuery] string id = "")
        {
            List<Account> accounts = null;

            accounts = await account_dbContext.Accounts
                .Where(account => account.Username.Contains(username))
                .Where(account => (!String.IsNullOrEmpty(id) && account.AccountId.Equals(id)) || 
                (String.IsNullOrEmpty(id) && !account.AccountId.Equals(id)))
                .ToListAsync();

            if (accounts == null)
            {
                return BadRequest();
            }

            return Ok(accounts);
        }

        [HttpGet("UserLogin")]
        public async Task<ActionResult<Account>> UserLogin([FromBody]string username = "", [FromQuery] string password = "")
        {
            Account userAccount = null;
            Password userPassword = null;
            try
            {
                userAccount = await account_dbContext.Accounts
                .Where(account => account.Username.Contains(username)).FirstOrDefaultAsync();

                if (userAccount != null)
                {
                    if(userAccount.Status.StatusId == 2)
                    {
                        return BadRequest("This user has been banned and cannot access SpotyMe");
                    }
                    else
                    {
                        userPassword = await account_dbContext.Passwords
                        .Where(p => p.PasswordString.Contains(password)).FirstOrDefaultAsync();

                        if(userPassword.PasswordString.Contains(password))
                        {
                            return Ok("Login Successful");
                        } 
                        else
                        {
                            return BadRequest("Incorrect password. Please try again");
                        }
                    }
                }
                else
                {
                    accountLog.LogError("LOGIN ACCOUNT ERROR: Username does not exist");
                    return BadRequest("ERROR: This username is not registered. Please try a different one");
                }
            }
            catch (Exception ex)
            {
                accountLog.LogError("LOGIN ACCOUNT EXCEPTION:\n" + ex.Message);
                return BadRequest(ex);
            }
        }
        
        [HttpPost("RegisterAccount")]
        public async Task<ActionResult<Account>> RegisterAccount([FromBody]Account newAccount)
        {
            if(newAccount == null)
            {
                accountLog.LogError("CREATE ACCOUNT ERROR: Fields cannot be empty");
                return BadRequest("The fields for the new account are empty. Please fill all fields and try again");
            }
            else
            {
                Account proxyAccount = null;

                try
                {                  
                    proxyAccount = await account_dbContext.Accounts
                    .Where(duplicate => duplicate.Email.Contains(newAccount.Email)).FirstOrDefaultAsync();
                    
                    if(proxyAccount == null)
                    {
                        bool isIdDuplicate = true;
                        string id = Guid.NewGuid().ToString();
                        newAccount.AccountId = id;

                        do
                        {
                            proxyAccount = await account_dbContext.Accounts
                            .Where(duplicate => duplicate.AccountId.Contains(newAccount.AccountId)).FirstOrDefaultAsync();

                            if(proxyAccount == null)
                            {
                                isIdDuplicate = false;                     
                            }
                            else
                            {
                                newAccount.AccountId = Guid.NewGuid().ToString();
                            }
                            
                        } while(isIdDuplicate);

                        account_dbContext.Entry(newAccount).State = EntityState.Added;
                        account_dbContext.AddRange(newAccount.Passwords);
                        await account_dbContext.SaveChangesAsync();
                        accountLog.LogInformation("REGISTER ACCOUNT SUCCESSFUL: {0}", newAccount.Username);
                        return Created("", newAccount);
                    }    
                    else
                    {
                        accountLog.LogError("CREATE ACCOUNT ERROR: Account Email cannot be a duplicate");
                        return BadRequest("ERROR: This email is already used for affiliated to another account. Please use a different one");
                    }  
                }
                catch (Exception ex)
                {
                    accountLog.LogError("REGISTER EXCEPTION:\n" + ex.Message);
                    return BadRequest(ex);
                }
            }      
        }

        [HttpPut("UpdateAccount")]
        public async Task<ActionResult<Account>> UpdateAccount([FromBody]Account account)
        {
            if(account == null)
            {
                accountLog.LogError("UPDATE ERROR: The account was not found");
                return BadRequest("ERROR: The account to update could not be found.");
            }

            try
            {
                var selectedAccount = account_dbContext.Accounts.SingleOrDefault(a => a.AccountId == account.AccountId);
                selectedAccount = account;
                account_dbContext.Entry(selectedAccount).State = EntityState.Modified;
                await account_dbContext.SaveChangesAsync();
                
                accountLog.LogInformation("UPDATE ACCOUNT SUCCESSFUL: {0}", account.Username);
                return Ok(account);
            }
            catch (Exception ex)
            {
                accountLog.LogError("UPDATE EXCEPTION:\n" + ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPut("BanAccount")]
        public async Task<ActionResult<Account>> banAccount([FromBody]Account account)
        {
            if(account == null)
            {
                accountLog.LogError("SEARCH ERROR: The account was not found");
                return BadRequest("ERROR: The account to ban could not be found.");
            }

            try
            {
                var selectedAccount = account_dbContext.Accounts.SingleOrDefault(a => a.AccountId == account.AccountId);
                selectedAccount = account;
                selectedAccount.StatusId = 2;
                account_dbContext.Entry(selectedAccount).State = EntityState.Modified;
                await account_dbContext.SaveChangesAsync();
                
                accountLog.LogInformation("ACCOUNT BAN SUCCESSFUL: {0}", selectedAccount.Username);
                return Ok(account);
            }
            catch (Exception ex)
            {
                accountLog.LogError("BAN EXCEPTION:\n" + ex.Message);
                return BadRequest(ex);
            }
        }
    }
}