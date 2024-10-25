using Invoices.Data;
using Invoices.Dto;
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
        // ID ile kullanıcı seçme API'si
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            return Ok(user);
        }
        // Yeni kullanıcı oluşturma
        [HttpPost]
        public ActionResult<User> CreateUser(DtoAddUser dto)
        {
            var user = new User
            {
                
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                City = dto.City,
                PostCode = dto.PostCode,
                Country = dto.Country,
                StreetAddress = dto.StreetAddress,
                IsDeleted = false
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        // Kullanıcı güncelleme
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, DtoAddUser dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);

            if (user == null)
            {
                return NotFound(new { message = "Güncellenecek kullanıcı bulunamadı." });
            }

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.City = dto.City;
            user.PostCode = dto.PostCode;
            user.Country = dto.Country;
            user.StreetAddress = dto.StreetAddress;

            _context.SaveChanges();

            return NoContent();
        }

        // Kullanıcı silme (yumuşak silme)
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);

            if (user == null)
            {
                return NotFound(new { message = "Silinecek kullanıcı bulunamadı." });
            }

            user.IsDeleted = true;
            _context.SaveChanges();

            return NoContent();
        }
    }
}
