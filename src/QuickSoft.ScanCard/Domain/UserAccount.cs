namespace QuickSoft.ScanCard.Domain
{
    using System;

    public class UserAccount
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string ProfileUrl { get; set; }

        public int UserType { get; set; }

        public DateTime CreatedDate { get; set; } = new();
    }
}