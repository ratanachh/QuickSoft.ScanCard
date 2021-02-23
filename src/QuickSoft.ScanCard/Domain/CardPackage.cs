namespace QuickSoft.ScanCard.Domain
{
    using System;

    public class CardPackage
    {
        public int Id { get; set; }

        public int ServiceAmount { get; set; }

        public bool IsActive { get; set; }

        public decimal Price { get; set; }

        public Card Card { get; set; }

        public Audit Audit { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}