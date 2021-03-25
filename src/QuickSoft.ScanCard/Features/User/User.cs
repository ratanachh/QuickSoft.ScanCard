namespace QuickSoft.ScanCard.Features.User
{
    public class User
    {
        public string Username { get; set; }
        
        public string ProfileUrl { get; set; }

        public string Token { get; set; }

        public string Phone { get; set; }

        public int UserType { get; set; }
    }

    public record UserEnvelope(User User);
}