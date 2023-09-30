using System.Security.Claims;

namespace TicketingSystem.Models;

public class UserSession
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Firstname { get; set; }
    public List<string> Roles { get; set; } = new();

    public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[]
    {
        new(ClaimTypes.Email, Email),
        new(ClaimTypes.Hash, Password),
        new(nameof(Firstname), Firstname)
    }.Concat(Roles.Select(role => new Claim(ClaimTypes.Role, role)).ToArray()),
        "Authentication"));

    public static UserSession FromClaimsPrincipal(ClaimsPrincipal claimsPrincipal) => new()
    {
        Email = claimsPrincipal.FindFirstValue(ClaimTypes.Email),
        Password = claimsPrincipal.FindFirstValue(ClaimTypes.Hash),
        Firstname = claimsPrincipal.FindFirstValue(nameof(Firstname)),
        Roles = claimsPrincipal.FindAll(ClaimTypes.Role).Select(role => role.Value).ToList()
    };
}