namespace RssToWordpressXmlRpcPoster.Models.RssUptime
{
    public class RssWithUrl
    {

        public RssWithUrl(RssModel item, string url)
        {
            this.RssModel = item;
            this.RequestUrl = url;
        }
        public RssModel RssModel { get; set; }
        public ReaderApiParserJson ParsedJson { get; set; }
        public string RequestUrl { get; set; }
    }
}