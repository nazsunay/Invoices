using Invoices.Data;
using Invoices.Dto;
using Invoices.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("filter")]
        public IActionResult GetClientByName([FromQuery] string name)
        {
            var invoices = _context.Clients
                                   .Where(i => i.Name == name)
                                   .ToList();
            return Ok(invoices);
        }


        [HttpGet("All")]
        public ActionResult<List<Client>> GetAllClients()
        {
            var clients = _context.Invoices
        .Include(i => i.Client)  // Client bilgilerini dahil ediyoruz
        .Where(i => i.Client != null) // Client bilgisi olan faturalara filtre uygular
        .Select(i => new
        {
            InvoiceId = i.InvoiceId,
            Client = new
            {
                i.Client.Id,
                i.Client.Name,
                i.Client.Email,
                i.Client.Phone,
                i.Client.City,
                i.Client.PostCode,
                i.Client.Country,
                i.Client.StreetAddress
            }
        })
        .ToList();

            return Ok(clients);
        }

        [HttpGet("{id}")]
        public ActionResult<Client> GetClientById(int id)
        {
            var client = _context.Clients
                                .Include(c => c.Invoices)
                                .FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }


        [HttpPost("Create")]
        public IActionResult CreateClient([FromBody] DtoAddClient clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = new Client
            {
                Name = clientDto.Name,
                Email = clientDto.Email,
                Phone = clientDto.Phone,
                City = clientDto.City,
                PostCode = clientDto.PostCode,
                Country = clientDto.Country,
                StreetAddress = clientDto.StreetAddress,
            };

            _context.Clients.Add(client);
            _context.SaveChanges();


            return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, client);
        }


        [HttpPut("Update/{id}")]
        public IActionResult UpdateClient(int id, [FromBody] Client updatedClient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            client.Name = updatedClient.Name;
            client.Email = updatedClient.Email;
            client.Phone = updatedClient.Phone;
            client.City = updatedClient.City;
            client.PostCode = updatedClient.PostCode;
            client.Country = updatedClient.Country;
            client.StreetAddress = updatedClient.StreetAddress;

            _context.Clients.Update(client);
            _context.SaveChanges();

            return Ok(client);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteClient(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            _context.SaveChanges();

            return Ok();
        }
    }
}
