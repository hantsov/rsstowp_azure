using System;
using System.Collections.Generic;
using System.Linq;
using RssToWordpressXmlRpcPoster.Models.RssUptime;
using WordPressSharp;
using WordPressSharp.Constants;
using WordPressSharp.Models;

namespace RssToWordpressXmlRpcPoster.Models
{
    /// <summary>
    /// Uses WordPressSharp to post and read from
    /// defined Wordpress site.
    /// </summary>
    public class WordPress
    {
        private WordPressSiteConfig config;

        public WordPress(string username, string password, string site, int blogid)
        {
            
            config = new WordPressSiteConfig
            {
                BaseUrl = site,
                BlogId = blogid,
                Username = username,
                Password = password
            };
        }

        public Post[] GetPosts()
        {
            Post[] posts;
            using (var client = new WordPressClient(config))
            {
                //PostFilter filter = new PostFilter();
                //filter.PostType = PostType.Post;
                //filter.PostStatus = PostStatus.Any;
                posts = client.GetPosts(null);
            }
            return posts;
        }
        public void MakeNewPost(RssWithUrl postData)
        {
            var newPost = new Post
            {
                Title = postData.ParsedJson.Title,
                PublishDateTime = DateTime.Parse(postData.RssModel.PubDate),
                Content = postData.ParsedJson.Content + "<p>" + "<a href =\"" + postData.RssModel.Link + "\">" + "To original site.." + "</a></p>",
                Terms = GetRssCategory(),
                PostType = "post",
                Status = "publish"
            };
            using (var client = new WordPressClient(config))
            {
                client.NewPost(newPost);
            }
        }

        // should do some refactoring...also this checks every post seperately but should only happen once per update
        public Term[] GetRssCategory()
        {
            // from https://github.com/abrudtkuhl/WordPressSharp/issues/48#issuecomment-105643517

            var keyword = new Term
            {
                Name = "RssFeedPosts",
                Slug = "Something, something...",
                Taxonomy = "category"
            };
            var existingTerm = CheckForCategory(keyword);
            if (existingTerm == null)
            {
                using (var client = new WordPressClient(config))
                {
                    keyword.Id = client.NewTerm(keyword);
                }
            }
            else
                keyword = existingTerm;
            //create a terms list
            var terms = new List<Term>();
            terms.Add(keyword);
            return terms.ToArray();
        }

        private Term CheckForCategory(Term keyword)
        {
            Term existingTerm;
            using (var client = new WordPressClient(config))
            {
                var terms = client.GetTerms("category", new TermFilter());
                existingTerm = terms.FirstOrDefault(x => x.Name.Equals(keyword.Name));
            }
            return existingTerm;
        }
    }
}
