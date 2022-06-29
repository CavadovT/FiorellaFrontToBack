using FrontToBack.Models;
using System.Collections.Generic;

namespace FrontToBack.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public SliderContent SliderContent { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public About About { get; set; }
        public List<Icon>Icons { get; set; }
        public List<Expert> Experts { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Say> Says { get; set; }
        public List<Instagram> Instagrams { get; set; }
    }
}
