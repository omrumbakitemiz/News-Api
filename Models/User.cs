using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace News.Api.Models
{
    public class User : IdentityUser
    {
        public ICollection<UserNews> UserNews { get; set; }
    }
}