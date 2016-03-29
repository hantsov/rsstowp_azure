using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Configuration;
using RssToWordpressXmlRpcPoster.Models;

namespace RssToWordpressXmlRpcPoster
{
    class Program
    {
        static void Main(string[] args)
        {

            Publish(GetUserData());

        }

        private static UserData GetUserData()
        {
            var rawuUserdata = ConfigurationManager.GetSection("userdata") as NameValueCollection;
            var userdata = new UserData();
            if (userdata != null)
            {
                userdata.FeedUrl = rawuUserdata.Get("FeedUrl");
                userdata.ReaderToken = rawuUserdata.Get("ReaderToken");
                userdata.WpPassword = rawuUserdata.Get("WpPassword");
                userdata.WpUrl = rawuUserdata.Get("WpUrl");
                userdata.WpUser = rawuUserdata.Get("WpUser");
                userdata.WpBlogId = int.Parse(rawuUserdata.Get("WpBlogId"));

            }
            return userdata;

        }

        private static void Publish(UserData userdata)
        {
            var errors = new List<string>();
            try
            {
                Console.WriteLine("Please wait for completion...");
                RssFeedToWP rssToWp = new RssFeedToWP(userdata);

                var newPosts = rssToWp.GetNonDuplicatePosts();
                if (newPosts == null)
                {
                    errors.Add("Something went wrong in getting data from either the feed or Readability. Check your keys and URL-s");
                    DisplayErrors(errors);
                }
                else if (newPosts.Count > 0)
                {
                    Console.WriteLine("Found " + newPosts.Count + " new posts to publish. Publishing...");
                    foreach (var post in newPosts)
                    {
                        rssToWp.wpClient.MakeNewPost(post);
                    }
                }
                else
                    Console.WriteLine("No new posts.");
            }
            catch (Exception e)
            {
                errors.Add(e.ToString());
                DisplayErrors(errors);
            }
            Console.WriteLine("Done!");
        }

        private static void DisplayErrors(List<string> errors)
        {
            int i = 1;
            foreach (var error in errors)
            {
                Console.WriteLine("Following errors occurred, try again: ");
                Console.WriteLine(i + ". " + error);

            }
            Environment.Exit(0);
        }
    }
}
