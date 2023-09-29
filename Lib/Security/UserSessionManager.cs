using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace TicketingSystem.Lib.Security;

public class UserSessionManager
{    
    private readonly IJSRuntime _jsRuntime;
    private const string SessionKey = "UserSession";

    public UserSessionManager(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetUserSession(string Email, string Department, int ID)
    {
        Models.UserSession session = new Models.UserSession { Email = Email, Department = Department, ID = ID };
        var sessionJson = JsonSerializer.Serialize(session);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", SessionKey, sessionJson);
    }

    public async Task<UserSession> GetUserSession()
    {
        var sessionJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", SessionKey);
        return string.IsNullOrWhiteSpace(sessionJson)
            ? null
            : JsonSerializer.Deserialize<UserSession>(sessionJson);
    }

    public async Task ClearUserSession()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", SessionKey);
    }
}