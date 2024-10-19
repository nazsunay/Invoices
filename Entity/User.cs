﻿namespace Invoices.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string StreetAddress { get; set; }

       
        public List<Invoice> Invoices { get; set; } = new List<Invoice>();
    }


    

   
    
    
}
