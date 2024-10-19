using System.ComponentModel.DataAnnotations;

namespace Invoices.Entity
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(150)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [Required]
        [MaxLength(50)]
        public string PostCode { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }
        [MaxLength(150)]
        public string StreetAddress { get; set; }

       
        public List<Invoice> Invoices { get; set; } = new List<Invoice>();
    }


    

   
    
    
}
