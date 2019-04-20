using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.JsonWebTokens;
using News.Api.Models;

namespace News.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class NewsController : Controller
    {
        private readonly NewsDbContext _context;
        private readonly IHubContext<NewsHub> _hubContext;

        public NewsController(NewsDbContext context, IHubContext<NewsHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult GetNews()
        {
            var news = _context.Set<Models.News>().ToList();
            return Ok(news);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleNews(string id)
        {
            var foundedNews = await FindNews(id);

            return Ok(foundedNews);
        }

        [HttpPost]
        public async Task<IActionResult> AddNews([FromBody] Models.News news)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("AddNews", news);

            return Ok(news);
        }

        [HttpGet("newsTypes")]
        public IActionResult GetAllNewsTypes()
        {
            var newsTypes = Enum.GetValues(typeof(NewsType));

            return Ok(newsTypes);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(string id)
        {
            var foundedNews = await FindNews(id);

            _context.News.Remove(foundedNews);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("DeleteNews", (Models.News) foundedNews);

            return Ok();
        }

        [HttpGet("like/{id}")]
        public async Task<IActionResult> LikeNews(string id)
        {
            var foundedNews = await FindNews(id) as Models.News;
            var (canLike, userId) = CanLikeOrDislikeNews(foundedNews);
            if (!canLike) return Forbid();

            foundedNews.LikeCount++;
            foundedNews.UserNews.Add(new UserNews {UserId = userId});
            
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("LikeNews", foundedNews);

            return Ok(foundedNews);
        }

        [HttpGet("dislike/{id}")]
        public async Task<IActionResult> DislikeNews(string id)
        {
            var foundedNews = await FindNews(id) as Models.News;
            var (canDislike, userId) = CanLikeOrDislikeNews(foundedNews);
            if (!canDislike) return Forbid();

            foundedNews.DislikeCount++;
            foundedNews.UserNews.Add(new UserNews {UserId = userId});

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("DislikeNews", foundedNews);

            return Ok(foundedNews);
        }

        [HttpGet("view/{id}")]
        public async Task<IActionResult> IncreaseViewCount(string id)
        {
            var foundedNews = await FindNews(id);

            foundedNews.ViewCount++;
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ViewNews", (Models.News) foundedNews);

            return Ok(foundedNews);
        }

        private async Task<dynamic> FindNews(string id)
        {
            var foundedNews = await _context.News.FindAsync(id);
            if (foundedNews == null) return false;
            return foundedNews;
        }

        private (bool, string) CanLikeOrDislikeNews(Models.News news)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            var foundedUserNews = _context.Set<UserNews>().Where(userNews => userNews.NewsId == news.Id)
                .Where(userNews => userNews.UserId == userId).Any();

            return (!foundedUserNews, userId);
        }
    }
}