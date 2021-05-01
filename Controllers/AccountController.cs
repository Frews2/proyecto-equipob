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
using MSCuenta.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MSCuenta.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private AccountsContext account_dbContext;
        private readonly ILogger<AccountController> accountLog;
        private const int BANNED = 2;
        private const int ADMIN = 3;

        public AccountController(ILogger<AccountController> logger) 
        {
            accountLog = logger;
            account_dbContext = new AccountsContext();
        }

        [HttpGet("SearchAccount")]
        public async Task<ActionResult<JObject>> SearchAccount([FromQuery]string username = "", [FromQuery] string id = "")
        {
            JObject returnObject;
            List<Account> accounts = null;

            accounts = await account_dbContext.Accounts
                .Where(account => account.Username.Contains(username))
                .Where(account => (!String.IsNullOrEmpty(id) && account.AccountId.Equals(id)) || 
                (String.IsNullOrEmpty(id) && !account.AccountId.Equals(id)))
                .ToListAsync();

            if (accounts.Count < 1)
            {
                string errorMessage  = "No account was found";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }
            else
            {
                returnObject = JSONFormatter.SuccessMessageFormatter("Account found", accounts);
                return Ok(returnObject);
            }
        }

        [HttpGet("UserLogin")]
        public async Task<ActionResult<JObject>> UserLogin([FromQuery]string username = "", [FromQuery] string password = "")
        {
            JObject returnObject;
            Account userAccount = null;
            Password userPassword = null;
            try
            {
                userAccount = await account_dbContext.Accounts
                .Where(account => account.Username.Contains(username)).FirstOrDefaultAsync();

                if (userAccount != null)
                {
                    if(userAccount.StatusId == 2)
                    {
                        string errorMessage  = "This user has been banned and cannot access SpotyMe";
                        returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                        return BadRequest(returnObject);
                    }
                    else
                    {
                        userPassword = await account_dbContext.Passwords
                        .Where(p => p.PasswordString.Contains(password)).FirstOrDefaultAsync();

                        if(userPassword.PasswordString.Contains(password) && userPassword.OwnerId.Equals(userAccount.AccountId))
                        {
                            
                            returnObject = JSONFormatter.SuccessMessageFormatter("Login successful", userAccount);
                            return Ok(returnObject);
                        } 
                        else
                        {
                            string errorMessage = "Incorrect password. Please try again";
                            returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                            return BadRequest(returnObject);
                        }
                    }
                }
                else
                {
                    accountLog.LogError("LOGIN ACCOUNT ERROR: Username does not exist");
                    string errorMessage = "ERROR: This username is not registered. Please try a different one";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
            }
            catch (Exception ex)
            {
                accountLog.LogError("LOGIN ACCOUNT EXCEPTION:\n" + ex.Message);
                returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                return BadRequest(returnObject);
            }
        }
        
        [HttpPost("RegisterAccount")]
        public async Task<ActionResult<JObject>> RegisterAccount([FromBody]Account newAccount)
        {
            JObject returnObject;

            if(newAccount == null)
            {
                accountLog.LogError("CREATE ACCOUNT ERROR: Fields cannot be empty");
                string errorMessage = "The fields for the new account are empty. Please fill all fields and try again";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
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

                            if (proxyAccount == null)
                            {
                                isIdDuplicate = false;
                            }
                            else
                            {
                                newAccount.AccountId = Guid.NewGuid().ToString();
                            }

                        } while (isIdDuplicate);

                        Password newPassword = newAccount.Passwords.FirstOrDefault();
                        Password proxyPassword = null;
                        isIdDuplicate = true;
                        newPassword.PasswordId = Guid.NewGuid().ToString();

                        do
                        {
                            proxyPassword = await account_dbContext.Passwords
                            .Where(duplicate => duplicate.PasswordId.Contains(newPassword.PasswordId)).FirstOrDefaultAsync();

                            if (proxyPassword == null)
                            {
                                isIdDuplicate = false;
                            }
                            else
                            {
                                newPassword.PasswordId = Guid.NewGuid().ToString();
                            }

                        } while (isIdDuplicate);

                        newPassword.OwnerId = newAccount.AccountId;
                        newAccount.Passwords.Add(newPassword);
                        account_dbContext.Entry(newAccount).State = EntityState.Added;
                        account_dbContext.AddRange(newAccount.Passwords);
                        await account_dbContext.SaveChangesAsync();
                        accountLog.LogInformation("REGISTER ACCOUNT SUCCESSFUL: {0}", newAccount.Username);
                        returnObject = JSONFormatter.SuccessMessageFormatter("Account registered successfully", newAccount);
                        return Ok(returnObject);
                    }    
                    else
                    {
                        accountLog.LogError("CREATE ACCOUNT ERROR: Account Email cannot be a duplicate");
                        string errorMessage = "ERROR: This email is already used for another account. Please use a different one";
                        returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                        return BadRequest(returnObject);
                    }  
                }
                catch (Exception ex)
                {
                    accountLog.LogError("REGISTER EXCEPTION:\n" + ex.Message);
                    returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                    return BadRequest(returnObject);
                }
            }      
        }

        [HttpPut("UpdateAccount")]
        public async Task<ActionResult<JObject>> UpdateAccount([FromBody]Account account)
        {
            JObject returnObject;

            if(account == null)
            {
                accountLog.LogError("UPDATE ERROR: The account was not found");
                string errorMessage = "ERROR: The account to update could not be found.";
                returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                return BadRequest(returnObject);
            }

            Account selectedAccount = null;
            bool noEmailChange = true;
            try
            {
                selectedAccount =  await account_dbContext.Accounts
                .Where(a => a.AccountId.Contains(account.AccountId))
                .FirstOrDefaultAsync();

                if(!selectedAccount.Email.Contains(account.Email))
                {
                    noEmailChange = false;
                    selectedAccount.Email =  account.Email;
                }

                selectedAccount.Username = account.Username;

                if(selectedAccount != null)
                {
                    Account proxyAccount = null;
                    proxyAccount = await account_dbContext.Accounts
                    .Where(duplicate => duplicate.Email.Contains(account.Email)).FirstOrDefaultAsync();

                    if (noEmailChange || proxyAccount == null)
                    {
                        account_dbContext.Entry(selectedAccount).State = EntityState.Modified;
                        await account_dbContext.SaveChangesAsync();

                        accountLog.LogInformation("UPDATE ACCOUNT SUCCESSFUL: {0}", account.Username);
                        returnObject = JSONFormatter.SuccessMessageFormatter("Account update successful", selectedAccount);
                        return Ok(returnObject);
                    }
                    else
                    {
                        accountLog.LogError("UPDATE ACCOUNT ERROR: Account Email cannot be a duplicate");
                        string errorMessage = "ERROR: This email is already used for another account. Please use a different one";
                        returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                        return BadRequest(returnObject);
                    }
                }
                else
                {
                    string errorMessage  = "Could not locate account to update";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                
                
            }
            catch (Exception ex)
            {
                accountLog.LogError("UPDATE EXCEPTION:\n" + ex.Message);
                returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                return BadRequest(returnObject);
            }
        }

        [HttpPut("BanAccount")]
        public async Task<ActionResult<JObject>> banAccount([FromQuery]string accountId)
        {
            JObject returnObject;

            try
            {
                var selectedAccount = account_dbContext.Accounts.SingleOrDefault(a => a.AccountId == accountId);

                if(selectedAccount == null)
                {
                    accountLog.LogError("SEARCH ERROR: The account was not found");
                    string errorMessage = "ERROR: The account to ban could not be found.";
                    returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                    return BadRequest(returnObject);
                }
                else
                {
                    if(selectedAccount.StatusId == BANNED)
                    {
                        string errorMessage = "ERROR: This account has already been banned.";
                        returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                        return BadRequest(returnObject);
                    }
                    else
                    {
                        if (selectedAccount.StatusId == ADMIN)
                        {
                            string errorMessage = "ERROR: Administrator account cannot be banned.";
                            returnObject = JSONFormatter.ErrorMessageFormatter(errorMessage);
                            return BadRequest(returnObject);
                        }
                        else
                        {
                            selectedAccount.StatusId = BANNED;
                            account_dbContext.Entry(selectedAccount).State = EntityState.Modified;
                            await account_dbContext.SaveChangesAsync();
                            accountLog.LogInformation("ACCOUNT BAN SUCCESSFUL: {0}", selectedAccount.Username);
                            returnObject = JSONFormatter.SuccessMessageFormatter("Account banned successfully", selectedAccount);
                            return Ok(returnObject);
                        }
                    } 
                }  
            }
            catch (Exception ex)
            {
                accountLog.LogError("BAN EXCEPTION:\n" + ex.Message);
                returnObject = JSONFormatter.ErrorMessageFormatter(ex.Message);
                return BadRequest(returnObject);
            }
        }
    }
}