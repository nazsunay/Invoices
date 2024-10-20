using Invoices.Data;
using Invoices.Dto;
using Invoices.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace Invoices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InvoiceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("AllList")]
        public ActionResult<List<Invoice>> GetAllInvoices()
        {
            var invoices = _context.Invoices
              .Include(i => i.Payments)
              .Include(i => i.InvoiceItems)
              .Select(i => new {
                  i.InvoiceId,
                  i.InvoiceDate,
                  i.DueDate,
                  i.TotalAmount,
                  i.Status,
                  User = new
                  {
                      i.User.UserId,
                      i.User.Name,
                  }


              })
              .ToList(); ;

            return Ok(invoices);
        }

        [HttpGet("/{id}")]
        public IActionResult GetInvoiceById(int id)
        {
            var invoice = _context.Invoices.FirstOrDefault(i => i.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }

        [HttpGet("filter")]
        public IActionResult GetInvoicesByStatus([FromQuery] string status)
        {
            var invoices = _context.Invoices
                                   .Where(i => i.Status == status)
                                   .ToList();
            return Ok(invoices);
        }

        // 1. Fatura Ekleme (Create)
        [HttpPost("Create")]
        public IActionResult CreateInvoice([FromBody] DtoAddInvoice invoiceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var invoice = new Invoice
            {
                UserId = invoiceDto.UserId,
                InvoiceDate = invoiceDto.InvoiceDate,
                DueDate = invoiceDto.DueDate,
                TotalAmount = invoiceDto.TotalAmount,
                Status = invoiceDto.Status
            };

            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            
            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.InvoiceId }, invoice);
        }

        //  Fatura Güncelleme (Update)
        [HttpPut("/Update/{id}")]
        public IActionResult UpdateInvoice(int id, [FromBody] DtoAddInvoice invoiceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var invoice = _context.Invoices.Find(id);
            if (invoice == null)
            {
                return NotFound($" {id} not found.");
            }

            
            invoice.UserId = invoiceDto.UserId;
            invoice.InvoiceDate = invoiceDto.InvoiceDate;
            invoice.DueDate = invoiceDto.DueDate;
            invoice.TotalAmount = invoiceDto.TotalAmount;
            invoice.Status = invoiceDto.Status;

            _context.Invoices.Update(invoice);
            _context.SaveChanges();

            
            return Ok(invoice);
        }


        // 6. Fatura Sil
        [HttpDelete("{id}")]
        public IActionResult DeleteInvoice(int id)
        {
            var invoice = _context.Invoices.FirstOrDefault(i => i.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            _context.Invoices.Remove(invoice);
            _context.SaveChanges();

            return Ok();
        }


    }
}

