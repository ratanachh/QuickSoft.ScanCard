using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickSoft.ScanCard.Domain
{
    using System;

    public class Card
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        public string CardNumber { get; set; }

        public bool IsActive { get; set; }
        
        [JsonIgnore]
        public int CustomerId { get; set; }
        

        [JsonIgnore]
        public int AuditId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}