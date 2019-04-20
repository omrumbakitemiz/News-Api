using System.ComponentModel.DataAnnotations.Schema;

namespace News.Api.Models
{
    [Table("UserNews")]
    public class UserNews
    {
        public string UserId { get; set; }
        public string NewsId { get; set; }
        public User User { get; set; }
        public News News { get; set; }
    }
}