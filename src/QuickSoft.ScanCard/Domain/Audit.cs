using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuickSoft.ScanCard.Domain
{
    using System;

    public class Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [StringLength(255)]
        [Column(TypeName = "NVARCHAR")]
        public string Descriptions { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        [JsonIgnore]
        public int PersonId { get; set; }
        
        public Person Person { get; set; }
        
        [JsonIgnore]
        public int CardId { get; set; }
        
        public Card Card { get; set; }
    }
}