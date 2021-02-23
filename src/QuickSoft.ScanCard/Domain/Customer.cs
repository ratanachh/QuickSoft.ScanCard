namespace QuickSoft.ScanCard.Domain
{
    using System;

    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public DateTime CreatedDate { get; set; }

        public Audit Audit { get; set; }
    }
}