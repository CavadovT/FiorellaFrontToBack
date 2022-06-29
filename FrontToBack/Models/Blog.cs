using System;

namespace FrontToBack.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public DateTime dateTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
