using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TicketingSystem.Data;
using TicketingSystem.Models;

namespace TicketingSystem.Lib.Security;

public class UserSessionService
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly string storageKey = "session_identity";
    private readonly DataContext _dbContext;

    public UserSessionService(ProtectedLocalStorage protectedLocalStorage, DataContext dbContext)
    {
        _protectedLocalStorage = protectedLocalStorage;
        _dbContext = dbContext;
    }

    public UserSession? FindUserInDatabase(string email, string password)
    {
        var userFound = _dbContext.Users.SingleOrDefault(user => user.Email == email && user.Password == password);
        if (userFound == null)
        {
            return null;
        }
        var newUserSession = new UserSession()
        {
            ID = userFound.ID,
            Email = userFound.Email,
            Firstname = userFound.Firstname,
            Lastname = userFound.Lastname,
            Password = userFound.Password,
            Role = userFound.UserRole
        };
        return newUserSession;
    }

    public async Task PersistUserSessionInBrowser(UserSession userSession)
    {
        string userJson = JsonConvert.SerializeObject(userSession);
        await _protectedLocalStorage.SetAsync(storageKey, userJson);
    }

    public async Task<UserSession> FetchUserSessionFromStorage()
    {
        try
        {
            var storedSessionResult = await _protectedLocalStorage.GetAsync<string>(storageKey);

            if (storedSessionResult.Success && !string.IsNullOrEmpty(storedSessionResult.Value))
            {
                var userSession = JsonConvert.DeserializeObject<UserSession>(storedSessionResult.Value);
                return userSession;
            }

            return null;
        }
        catch (InvalidOperationException)
        {
            
        }

        return null;
        
    }

    public async Task ClearBrowserSessionData() => await _protectedLocalStorage.DeleteAsync(storageKey);
}