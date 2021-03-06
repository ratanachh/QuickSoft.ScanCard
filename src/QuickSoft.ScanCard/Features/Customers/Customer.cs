﻿
using System.Collections.Generic;

namespace QuickSoft.ScanCard.Features.Customers
{
    using System;
    public class Customer
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Phone { get; set; }

        public string CardNumber { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public int AuditId { get; set; }

        public bool IsCardActive { get; set; }
    }
    
    public record CustomerEnvelope(Customer Customer);
    public record CustomersEnvelope(List<Customer> Customers);
}