using System.Text.Json.Serialization;

namespace QuickSoft.ScanCard.Features.Users
{
    public class User
    {
        public string Username { get; set; }
        
        public string ProfileUrl { get; set; }

        public string Token { get; set; }

        public string Phone { get; set; }

        [JsonPropertyName("userType")]
        public string Type { get; set; }

        public bool IsCurrentUser { get; set; }
    }

    public record UserEnvelope(User User);
}