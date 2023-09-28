using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TicketingSystem.Data;
using TicketingSystem.Models;

namespace TicketingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TechnicianController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public TechnicianController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Technician technician)
        {
            if (await TechnicianExists(technician.email))
            {
                return Conflict("Technician with this email already exists.");
            }

            var newTechnician = new Technician
            {
                email = technician.email,
                firstname = technician.firstname,
                lastname = technician.lastname,
                department = technician.department
            };

            _dataContext.Technicians.Add(newTechnician);
            await _dataContext.SaveChangesAsync();

            return Ok("Registration successful.");
        }

        private async Task<bool> TechnicianExists(string email)
        {
            return await _dataContext.Technicians.AnyAsync(tech => tech.email.ToLower() == email.ToLower());
        }
    }
}