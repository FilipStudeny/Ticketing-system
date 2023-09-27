using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.Models;

public class Ticket
{
    public string TicketId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Severity { get; set; }
    public string ClientEmail { get; set; }
    public string ClientPhoneNumber { get; set; }
    public string TicketAdministrator { get; set; }
    public DateTime DueDate { get; set; }
    
}
