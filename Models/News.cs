using System;

namespace News.Api.Models
{
    public class News
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string image { get; set; }
        public DateTime PublishDate { get; set; }
        public int LikeCount { get; set; }
        public string Type { get; set; }
        public int DislikeCount { get; set; }
        public int ViewCount { get; set; }
    }
}