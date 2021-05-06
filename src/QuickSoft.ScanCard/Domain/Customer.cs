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

        public string Name { get; set; }

        public string Phone { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public int CardId { get; set; }
        public Card Card { get; set; }

        [JsonIgnore]
        public ICollection<Audit> Audits { get; set; }
    }
}