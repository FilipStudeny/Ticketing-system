using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using TicketingSystem.Models;

namespace TicketingSystem.Lib.Security;

public class UserSessionAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly UserSessionService _userSessionService;
    public UserSession currentUserSession { get; private set; } = new();

    public UserSessionAuthenticationStateProvider(UserSessionService userSessionService)
    {
        _userSessionService = userSessionService;
        AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
    }

    public async Task Login(string email, string password)
    {
        var claimsPrincipal = new ClaimsPrincipal();
        var user = _userSessionService.FindUserInDatabase(email, password);

        if (user != null)
        {
            await _userSessionService.PersistUserSessionInBrowser(user);
            claimsPrincipal = user.ToClaimsPrincipal();
        }
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public async Task Loggout()
    {
        await _userSessionService.ClearBrowserSessionData();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
    }

    // Handle revisit/refresh of a page
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var claimsPrincipal = new ClaimsPrincipal();
        var user = await _userSessionService.FetchUserSessionFromStorage();

        if (user is not null)
        {
            var userInDatabase = _userSessionService.FindUserInDatabase(user.Email, user.Password);

            if (userInDatabase  != null)
            {
                claimsPrincipal = userInDatabase.ToClaimsPrincipal();
                currentUserSession = userInDatabase;
            }

        }

        return new(claimsPrincipal);
    }

    private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
    {
        var authenticationState = await task;
        currentUserSession = UserSession.FromClaimsPrincipal(authenticationState.User);
    }

    public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
}