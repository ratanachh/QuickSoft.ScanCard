namespace QuickSoft.ScanCard.Domain
{
    using System;
    using System.Text.Json.Serialization;
    using System.ComponentModel.DataAnnotations;

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