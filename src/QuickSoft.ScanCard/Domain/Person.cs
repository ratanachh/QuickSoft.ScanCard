using System;
using System.Text.Json.Serialization;

namespace QuickSoft.ScanCard.Domain
{
    public class Person
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string Phone { get; set; }

        public string ProfileUrl { get; set; }

        public int UserType { get; set; }

        public DateTime CreatedDate { get; set; } = new();
    }

    public static class UserConstants
    {
        public const string Admin = nameof(Admin);
        public const string User = nameof(User);
        public const string Developer = nameof(Developer);
    }
}