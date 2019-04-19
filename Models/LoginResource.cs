namespace News.Api.Models
{
    public class LoginResource
    {
        public string Token { get; set; }
        
        public User User { get; set; }
    }
}