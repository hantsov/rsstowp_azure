using System;
using System.IO;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using RssToWordpressXmlRpcPoster.Models.RssUptime;

namespace RssToWordpressXmlRpcPoster.Services
{
    public class ReadabilityService
    {
        private static string TOKEN_KEY;
        private static string requestUrl = "https://www.readability.com/api/content/v1/parser?token=";

        public ReadabilityService(string token)
        {
            TOKEN_KEY = token;
            requestUrl += TOKEN_KEY;
        }

        private string MakeJsonRequest(string requestUrl)
        {
            string responseString = null;
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));

                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseString = sr.ReadToEnd();
                    }
                    return responseString;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public ReaderApiParserJson GetParsedJson(string request)
        {
            var data = MakeJsonRequest(request);
            if (data != null)
            {
                return JsonConvert.DeserializeObject<ReaderApiParserJson>(data);
            }
            return null;
        }

        public string GetParsingUrl(string siteUrl)
        {
            return requestUrl + "&url=" + siteUrl;
        }
    }
}