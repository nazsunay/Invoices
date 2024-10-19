using Invoices.Data;
using Invoices.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<User>> GetAllTrainers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }
    }
}
