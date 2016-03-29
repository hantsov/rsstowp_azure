using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RssToWordpressXmlRpcPoster.Models.RssUptime;
using RssToWordpressXmlRpcPoster.Services;

namespace RssToWordpressXmlRpcPoster.Models
{
    /// <summary>
    /// Centralizes functionality of getting data from
    /// Readability, your RSS-feed and reading/writing to
    /// Wordpress
    /// </summary>
    public class RssFeedToWP
    {

        private ReadabilityService readerService;
        private RssFeedService rssFeed;
        public WordPress wpClient;

        public RssFeedToWP(UserData userdata)
        {
            Initialize(userdata);
        }

        private void Initialize(UserData userdata)
        {
            readerService = new ReadabilityService(userdata.ReaderToken);
            rssFeed = new RssFeedService(userdata.FeedUrl);
            wpClient = new WordPress(userdata.WpUser, userdata.WpPassword, userdata.WpUrl, userdata.WpBlogId);
        }

        public List<RssWithUrl> PostsFromReadability(List<RssModel> feedPosts)
        {
            var postsWithReadability = new List<RssWithUrl>();
            foreach (var item in feedPosts)
            {
                var rssWithUrl = new RssWithUrl(item, readerService.GetParsingUrl(item.Link));
                rssWithUrl.ParsedJson = readerService.GetParsedJson(rssWithUrl.RequestUrl);
                if (rssWithUrl.ParsedJson == null)
                {
                    return null;
                }
                postsWithReadability.Add(rssWithUrl);
            }
            return postsWithReadability;
        }

        public List<RssWithUrl> GetNonDuplicatePosts()
        {
            var posts = wpClient.GetPosts();
            var feeditems = rssFeed.GetRssFeed();

            var latestPost = posts.OrderByDescending(item => item.PublishDateTime).FirstOrDefault();
            if (latestPost != null)
            {
                feeditems =
                    feeditems.Where(feeditem => DateTime.Parse(feeditem.PubDate) > latestPost.PublishDateTime)
                        .ToList();
            }
            var postsToAdd = PostsFromReadability(feeditems);
            // if not null, order from earliest to latest in case updating fails in the middle
            postsToAdd = postsToAdd?.OrderBy(post => DateTime.Parse(post.RssModel.PubDate)).ToList();
            return postsToAdd;
        }

    }
}
