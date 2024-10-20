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
        public ActionResult<List<object>> GetAllInvoices()
        {
            var invoices = _context.Invoices
                .Include(i => i.Payments)
                .Include(i => i.InvoiceItems)
                .ThenInclude(ii => ii.Item)
                .Select(i => new
                {
                    i.InvoiceId,
                    i.InvoiceDate,
                    i.DueDate,
                    i.TotalAmount,
                    i.Status,
                    User = new
                    {
                        i.User.UserId,
                        i.User.Name,
                        i.User.Email,
                    },
                    InvoiceItems = i.InvoiceItems.Select(ii => new
                    {
                        ii.Item.Id,
                        ii.Item.Name,
                        ii.Item.Price,
                        ii.Item.Quantity,
                        ii.Item.Total,
                        ii.Item.PaymentMethod
                    }).ToList(), // InvoiceItems içindeki Item verilerini al
                    Payments = i.Payments.Select(p => new
                    {
                        p.PaymentId,
                        p.Amount,
                        p.PaymentDate
                    }).ToList()
                })
                .ToList();

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
        [HttpGet("Items")]
        public ActionResult<List<Item>> GetAllItems()
        {
            var items = _context.Items.ToList();
            return Ok(items);
        }
        [HttpGet("Items/{id}")] //belirli itemları getirmesi için 
        public IActionResult GetItemById(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound($"Item with id {id} not found.");
            }

            return Ok(item);
        }

        [HttpPost("Items/Create")]
        public IActionResult CreateItem([FromBody] DtoAddItem itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = new Item
            {
                
                Name = itemDto.Name,
                Amount = itemDto.Amount,
                PaymentMethod = itemDto.PaymentMethod,
                Quantity = itemDto.Quantity,
                Price = itemDto.Price,
                Total = itemDto.Total
            };

            _context.Items.Add(item);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }


        [HttpPut("Items/Update/{id}")]
        public IActionResult UpdateItem(int id, [FromBody] Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingItem = _context.Items.FirstOrDefault(i => i.Id == id);
            if (existingItem == null)
            {
                return NotFound($"Item with id {id} not found.");
            }

            existingItem.Name = item.Name;
            existingItem.Amount = item.Amount;
            existingItem.PaymentMethod = item.PaymentMethod;
            existingItem.Quantity = item.Quantity;
            existingItem.Price = item.Price;
            existingItem.Total = item.Total;

            _context.Items.Update(existingItem);
            _context.SaveChanges();

            return Ok(existingItem);
        }
        [HttpDelete("Items/Delete/{id}")]
        public IActionResult DeleteItem(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound($"Item with id {id} not found.");
            }

            _context.Items.Remove(item);
            _context.SaveChanges();

            return Ok();
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

