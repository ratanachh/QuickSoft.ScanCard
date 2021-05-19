namespace QuickSoft.ScanCard.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Person
    {
        [Key]
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is obligatory")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name has to have at least 3 characters")]
        public string Username { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "Password is obligatory")]
        [StringLength(120, MinimumLength = 3, ErrorMessage = "Name has to have at least 3 characters")]
        public string Password { get; set; }
        
        [StringLength(15)]
        public string Phone { get; set; }
        
        public string ProfileUrl { get; set; }


        [Required(ErrorMessage = "UserType is obligatory")]
        public int UserType { get; set; }


        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public ICollection<Audit> Audits { get; set; }
    }

    public static class UserConstants
    {
        public const string Admin = nameof(Admin);
        public const string User = nameof(User);
        public const string Developer = nameof(Developer);

        public static string GetUserTypeString(int userType)
        {
            return userType switch
            {
                0 => User,
                1 => Admin,
                2 => Developer,
                _ => userType.ToString()
            };
        }
    }
}