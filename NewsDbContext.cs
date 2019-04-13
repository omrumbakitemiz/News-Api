using Microsoft.EntityFrameworkCore;

namespace News.Api
{
    public class NewsDbContext: DbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options)
            : base(options)
        { }

        public DbSet<Models.News> News { get; set; }
    }
}