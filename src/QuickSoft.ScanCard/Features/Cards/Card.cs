using System;

namespace QuickSoft.ScanCard.Features.Cards
{
    public class Card
    {
        public string CardNumber { get; set; }

        public bool IsActive { get; set; }
        
        public int CustomerId { get; set; }
        
        public int AuditId { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public record CardEnvelope(Card Card);
}