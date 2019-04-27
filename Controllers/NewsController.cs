using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.JsonWebTokens;
using News.Api.Models;
using Newtonsoft.Json.Linq;

namespace News.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class NewsController : Controller
    {
        private readonly NewsDbContext _context;
        private readonly IHubContext<NewsHub> _hubContext;
        private readonly IHttpClientFactory _clientFactory;

        public NewsController(NewsDbContext context, IHubContext<NewsHub> hubContext, IHttpClientFactory clientFactory)
        {
            _context = context;
            _hubContext = hubContext;
            _clientFactory = clientFactory;
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

            var json = @"{
              'notification': {
                'title': 'News',
                'body': 'New news arrived...',
                'sound': 'default',
                'click_action': 'FCM_PLUGIN_ACTIVITY',
                'icon': 'fcm_push_icon',
              },
              'data': {
                'newsId': 'adrenokortikotropik hormon',
              },
              'to': '/topics/all',
              'priority': 'high',
              'restricted_package_name': '',
            }";

            JObject requestBody = JObject.Parse(json);
            requestBody["data"]["newsId"] = news.Id;
            requestBody["notification"]["title"] = news.Title;

            var length = 30;
            if (news.Text.Length < length) length = news.Text.Length;
            requestBody["notification"]["body"] = news.Text.Substring(0, length).Insert(length, "...");

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization",
                "key=AAAAwUeco1c:APA91bFY4m-vuOPPCzcqb4upPh2Y8BTNSd7bc04enZ9vONOed9YJjytF5pha1FhgQw_JZb2aq4LKy06s2vtkcfePg_SvcX_noB0MILNFLhJQzuogAYNHHnbUm9x1l1JBs_dO_si53_IE");

            var response = await client.PostAsJsonAsync("https://fcm.googleapis.com/fcm/send", requestBody);
            var result = await response.Content.ReadAsStringAsync();

            return Ok(result);
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