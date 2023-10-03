
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.Models;

public class User
{
    public string ID { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public string UserRole { get; set; }
}