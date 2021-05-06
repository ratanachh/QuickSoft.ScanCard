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

        public string CardNumber { get; set; }

        public bool IsActive { get; set; }

        public Customer Customer { get; set; }

        [JsonIgnore]
        public ICollection<Audit> Audit { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}