using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using TicketingSystem.Models;

namespace TicketingSystem.Lib.Security;

public class UserSessionService
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly string storageKey = "session_identity";

    public UserSessionService(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
    }

    public UserSession? FindUserInDatabase(string email, string password)
    {
        var userFromDatabase = new List<UserSession>()
        {
            new()
            {
                Email = "admin@gmail.com",
                Password = "123",
                Firstname = "Admin"
            }
        };

        var userFound = userFromDatabase.SingleOrDefault(user => user.Email == email && user.Password == password);
        return userFound;
    }

    public async Task PersistUserSessionInBrowser(UserSession userSession)
    {
        string userJson = JsonConvert.SerializeObject(userSession);
        await _protectedLocalStorage.SetAsync(storageKey, userJson);
    }

    public async Task<UserSession> FetchUserSessionFromStorage()
    {
        var storedSessionResult = await _protectedLocalStorage.GetAsync<string>(storageKey);

        if (storedSessionResult.Success && !string.IsNullOrEmpty(storedSessionResult.Value))
        {
            var userSession = JsonConvert.DeserializeObject<UserSession>(storedSessionResult.Value);
            return userSession;
        }

        return null;
    }

    public async Task ClearBrowserSessionData() => await _protectedLocalStorage.DeleteAsync(storageKey);
}