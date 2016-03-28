namespace RssToWordpressXmlRpcPoster.Models.RssUptime
{
    public class ReaderApiParserJson
    {
        public string Domain { get; set; }
        public string Next_Page_Id { get; set; }
        public string Url { get; set; }
        public string Short_url { get; set; }
        public string Author { get; set; }
        public string Excerpt { get; set; }
        public string Direction { get; set; }
        public int Word_count { get; set; }
        public int Total_pages { get; set; }
        public string Content { get; set; }
        public string Date_published { get; set; }
        public object Dek { get; set; }
        public string Lead_image_url { get; set; }
        public string Title { get; set; }
        public int Rendered_pages { get; set; }
    }
}