using System.Text.Json.Serialization;

namespace QuickSoft.ScanCard.Features.Profiles
{
    public class Profile
    {
        public string Username { get; set; }
        
        public string ProfileUrl { get; set; }

        public string Phone { get; set; }
        
    }
    
    public record ProfileEnvelope(Profile Profile);
}