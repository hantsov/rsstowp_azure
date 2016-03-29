namespace RssToWordpressXmlRpcPoster.Models
{
    public class UserData
    {
        public string WpUser { get; set; }
        public string WpPassword { get; set; }
        public string WpUrl { get; set; }
        public string ReaderToken { get; set; }
        public string FeedUrl { get; set; }
        public int WpBlogId { get; set; }
    }
}
