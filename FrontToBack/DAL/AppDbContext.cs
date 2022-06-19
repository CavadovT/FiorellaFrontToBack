using FrontToBack.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderContent> SliderContents { get; set; }
    }
}
