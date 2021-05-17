using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickSoft.ScanCard.Domain
{
    using System;

    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public int AuditId { get; set; }
    }
}