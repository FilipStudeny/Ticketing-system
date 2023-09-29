using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace TicketingSystem.Lib.Security;

public class AuthenticationService : AuthenticationStateProvider
{
    private protected readonly ProtectedSessionStorage _sessionStorage;
    private ClaimsPrincipal _anonymousUsers = new ClaimsPrincipal(new ClaimsIdentity());

    public AuthenticationService(ProtectedSessionStorage sessionStorage)
    {
        this._sessionStorage = sessionStorage;
    }
    
    
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var userSessionsStorage = await _sessionStorage.GetAsync<UserSession>("user_Session");
        var userSession = userSessionsStorage.Success ? userSessionsStorage.Value : null;

        if (userSession == null){
            return await Task.FromResult(new AuthenticationState(_anonymousUsers));
        }

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, userSession.email),
            new Claim(ClaimTypes.Role, userSession.role)
        }, "customAuth"));
        
        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    public async Task UpdateAuthenticationState(UserSession userSession)
    {
        ClaimsPrincipal claimsPrincipal;
        if (userSession != null)
        {
            await _sessionStorage.SetAsync("user_Session", userSession);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userSession.email),
                new Claim(ClaimTypes.Role, userSession.role)
            }, "customAuth"));
        }
        else
        {
            await _sessionStorage.DeleteAsync("user_Session");
            claimsPrincipal = _anonymousUsers;
        }
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
}