using System.Security.Claims;

namespace TicketingSystem.Models;

public class UserSession
{
    public string ID { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string Role { get; set; }

    public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[]
    {
        new(ClaimTypes.NameIdentifier, ID),
        new(ClaimTypes.Email, Email),
        new(ClaimTypes.Hash, Password),
        new(nameof(Firstname), Firstname),
        new(nameof(Lastname), Lastname),
        new(ClaimTypes.Role, Role)
        
    },"Authentication"));

    public static UserSession FromClaimsPrincipal(ClaimsPrincipal claimsPrincipal) => new()
    {
        ID = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier),
        Email = claimsPrincipal.FindFirstValue(ClaimTypes.Email),
        Password = claimsPrincipal.FindFirstValue(ClaimTypes.Hash),
        Firstname = claimsPrincipal.FindFirstValue(nameof(Firstname)),
        Lastname = claimsPrincipal.FindFirstValue(nameof(Firstname)),
        Role = claimsPrincipal.FindFirstValue(ClaimTypes.Role)
    };
}