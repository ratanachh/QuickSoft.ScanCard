namespace QuickSoft.ScanCard.Domain
{
    using System;

    public class Audit
    {
        public int Id { get; set; }
        
        public string Descriptions { get; set; }
        
        public DateTime CreatedDate { get; set; } = new();
        
        public int UserId { get; set; }
    }
}