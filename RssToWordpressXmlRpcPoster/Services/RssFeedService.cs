using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using RssToWordpressXmlRpcPoster.Models.RssUptime;

namespace RssToWordpressXmlRpcPoster.Services
{
    public class RssFeedService
    {
        private string URL = "https://www.readability.com/rseero/latest/feed";

        public RssFeedService(string url)
        {
            if (!string.IsNullOrEmpty(url))
                URL = url;
        }

        //from https://dzone.com/articles/creating-basic-rss-reader
        public List<RssModel> GetRssFeed()
        {
            XDocument feedXml = XDocument.Load(URL);
            var feeds = from feed in feedXml.Descendants("item")
                        select new RssModel()
                        {
                            Title = feed.Element("title").Value,
                            Link = feed.Element("link").Value,
                            Description = feed.Element("description").Value,
                            PubDate = feed.Element("pubDate").Value
                        };
            return feeds.ToList();
        }
    }
}