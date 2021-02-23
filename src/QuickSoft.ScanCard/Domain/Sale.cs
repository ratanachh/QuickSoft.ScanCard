namespace QuickSoft.ScanCard.Domain
{
    using System;

    public class Sale
    {
        public int Id { get; set; }

        public int UnitNumber { get; set; }

        public Audit Audit { get; set; }

        public DateTime CreatedDate { get; set; }

        public CardPackage CardPackage { get; set; }
    }
}