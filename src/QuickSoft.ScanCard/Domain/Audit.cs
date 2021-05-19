namespace QuickSoft.ScanCard.Domain
{
    using System;
    using System.Text.Json.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [StringLength(255)]
        public string Descriptions { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        [JsonIgnore]
        public int PersonId { get; set; }
        
        public Person Person { get; set; }
    }
}