using System.Text.Json.Serialization;

namespace QuickSoft.ScanCard.Features.Profiles
{
    public class Profile
    {
        public string Username { get; set; }
        
        public string ProfileUrl { get; set; }

        public string Phone { get; set; }
        
        [JsonPropertyName("userType")]
        public string Type { get; set; }

        public bool IsCurrentUser { get; set; }
    }
}