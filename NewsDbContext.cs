using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using News.Api.Models;

namespace News.Api
{
    public class NewsDbContext: IdentityDbContext<User>
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options)
            : base(options)
        { }

        public DbSet<Models.News> News { get; set; }
    }
}