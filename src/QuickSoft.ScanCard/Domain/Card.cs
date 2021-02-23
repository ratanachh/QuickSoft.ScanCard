namespace QuickSoft.ScanCard.Domain
{
    using System;

    public class Card
    {
        public int Id { get; set; }

        public string CardNumber { get; set; }

        public bool IsActive { get; set; }

        public Customer Customer { get; set; }

        public Audit Audit { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}