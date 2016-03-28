using System;
using System.Collections.Generic;
using System.IO;
using RssToWordpressXmlRpcPoster.Models;

namespace RssToWordpressXmlRpcPoster
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<string> errors = new List<string>();
            //string path = CheckFilePath(errors);
            //int id = CheckBlogId(errors);
            //if (errors.Count == 0)
            //{
            //    Publish(path, id);
            //}
            //else
            //    DisplayErrors(errors);

            List<string> errors = new List<string>();
            //string path = CheckFilePath(errors);
            //int id = CheckBlogId(errors);
            string path = Directory.GetCurrentDirectory() + "/Keys.xml";
            //Console.WriteLine(path);
            int id = 1;
            if (errors.Count == 0)
            {
                Publish(path, id);
            }
            else
                DisplayErrors(errors);

        }

        private static int CheckBlogId(List<string> errors)
        {
            Console.WriteLine("Please enter the id of your blog (leave empty if default of 1, this is needed for multisite):");
            var blogid = Console.ReadLine();
            int id;
            if (!string.IsNullOrEmpty(blogid))
            {
                ;
                if (!int.TryParse(blogid, out id))
                {
                    errors.Add("Invalid id input");
                }
            }
            else
                id = 1;
            return id;
        }

        private static string CheckFilePath(List<string> errors)
        {
            Console.WriteLine("Please construct a Keys.xml file with specified structure " +
                                          "and type the path to that file here (leave empty if using default path):");
            var path = Console.ReadLine();


            if (string.IsNullOrEmpty(path))
            {
                path = Directory.GetCurrentDirectory() + "/Keys.xml";
            }
            if (!File.Exists(path))
            {
                errors.Add("File path invalid.");
                DisplayErrors(errors);
            }

            return path;
        }

        private static void Publish(string path, int id)
        {
            var errors = new List<string>();
            try
            {
                Console.WriteLine("Please wait for completion...");
                RssFeedToWP rssToWp = new RssFeedToWP(path, id);

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
            Console.ReadLine();
        }

        private static void DisplayErrors(List<string> errors)
        {
            int i = 1;
            foreach (var error in errors)
            {
                Console.WriteLine("Following errors occurred, try again: ");
                Console.WriteLine(i + ". " + error);

            }
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
